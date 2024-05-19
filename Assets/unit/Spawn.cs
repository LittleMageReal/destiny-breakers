using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Spawn : MonoBehaviour
{
    public Deck deck; 
    public Transform parentObject;
    public Transform artifact;
    public Transform effect;
    private int selectedCardIndex = 0; 

    private float scrollCooldown = 0.2f; // Time in seconds to wait after each scroll
    private float lastScrollTime; // Time when the last scroll occurred

    private float rightMouseHoldStartTime =  0f; // Time when the right mouse button was first pressed
    private bool isRightMouseHeld = false; // Whether the right mouse button is currently being held down


    // Define an event to notify when a card is selected
    public static event Action<int> OnCardSelected;

    public static Spawn instance;


    private new PhotonView photonView;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("More than one instance of Spawn found!");
        }
    }


    public void Update()
    {
        if (photonView.IsMine)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll != 0 && Time.time - lastScrollTime >= scrollCooldown)
            {
                lastScrollTime = Time.time;

                if (deck.hand.Count > 0)
                {
                    if (scroll > 0)
                    {
                        selectedCardIndex = (selectedCardIndex + 1) % deck.hand.Count;
                    }
                    else
                    {
                        selectedCardIndex = (selectedCardIndex - 1 + deck.hand.Count) % deck.hand.Count;
                    }

                    Card selectedCard = deck.hand[selectedCardIndex];
                }


                Debug.Log("number " + selectedCardIndex);
                
            }

        // Trigger the OnCardSelected event with the selected card index
        OnCardSelected?.Invoke(selectedCardIndex);


         // Card summoning and returning cards to deck
         if (Input.GetButtonDown("Fire2")) 
            {
                isRightMouseHeld = true;

                rightMouseHoldStartTime = Time.time;
            }

         if (Input.GetButtonUp("Fire2"))
            {
                if (Time.time - rightMouseHoldStartTime >= 3f)
                {
                    ReturnCardAndDrawNew();
                }
                else
                {
                    Card selectedCard = deck.hand[selectedCardIndex];
                    SpawnPrefab(selectedCard);
                }

                isRightMouseHeld = false;

            }
        }
    }

    public void SpawnPrefab(Card card)
    {
        if (card.isActive)
        {
           Debug.Log("This card is inactive and cannot be used.");
           return; // Exit the method if the card is inactive
        }

        if (DriftPointManager.Instance.SpendPoints(card.pointType, card.cardCost))
        {
            switch (card.spawnType)
            {
                case Card.spawnPosition.Stand:
                    SpawnAtMousePosition(card);
                    break;
                case Card.spawnPosition.Follow:
                    SpawnAtTransformPosition(card);
                    break;
                case Card.spawnPosition.Artifact:
                    SpawnOnSupportPosition(card);
                    break;
                case Card.spawnPosition.Effect:
                    SpawnBuffAndDebuff(card);
                    break;
            }

            if (!card.Move)
            {
                if (card.Token)
                   {
                     // If the card is a token card, remove it from the hand without adding it back to the deck
                     deck.hand.Remove(card);
                   }
                else
                {
                 // Remove the card from the hand
                 deck.hand.Remove(card);

                 // Add the card back to the deck
                 deck.deck.Add(card);

                }
                
            }
            
        }
        else
        {
            Debug.Log("Not enough points to spawn this unit");
        }

        void DestroySupportChildObjects()
        {
            foreach (Transform child in artifact.transform)
            {
                PhotonNetwork.Destroy(child.gameObject);
            }
        }

        void SpawnAtMousePosition(Card card)
        {
            // Create a ray from the camera through the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Use the hit position as the spawn position
                Vector3 spawnPosition = hit.point;
                // Set the y position to the same as the parent object's position
                //spawnPosition.y = parentObject.position.y;

                Ray downRay = new Ray(spawnPosition + Vector3.up, Vector3.down);
                if (Physics.Raycast(downRay, out RaycastHit downHit))
                {
                    // Set the y position to the hit position
                    spawnPosition.y = downHit.point.y;
                }

                // Instantiate the unit without a parent object
                GameObject instantiatedObject = PhotonNetwork.Instantiate("Cards/Card Models/" + card.unitPrefab.name, spawnPosition, transform.rotation);

                instantiatedObject.GetComponent<PhotonView>().TransferOwnership(parentObject.GetComponent<PhotonView>().Owner);
            }
        }

        void SpawnAtTransformPosition(Card card)
        {
            foreach (Transform child in transform) //destroy all chailds in object with this script
            {
                PhotonNetwork.Destroy(child.gameObject);
            }
            // Use the transform's position as the spawn position
            Vector3 spawnPosition = transform.position;

            // Instantiate the unit with a parent object
            GameObject instantiatedObject = PhotonNetwork.Instantiate("Cards/Card Models/" + card.unitPrefab.name, spawnPosition, transform.rotation);

            instantiatedObject.transform.SetParent(parentObject);

            instantiatedObject.GetComponent<PhotonView>().TransferOwnership(parentObject.GetComponent<PhotonView>().Owner);
        }

        void SpawnOnSupportPosition(Card card)
        {
            DestroySupportChildObjects();

            Vector3 spawnPosition = artifact.transform.position;
            // Instantiate the unit with the spawnOnGameObject as the parent object
            GameObject instantiatedObject = PhotonNetwork.Instantiate("Cards/Card Models/" + card.unitPrefab.name, spawnPosition, transform.rotation);

            instantiatedObject.transform.SetParent(artifact);

            instantiatedObject.GetComponent<PhotonView>().TransferOwnership(parentObject.GetComponent<PhotonView>().Owner);
        }

        void SpawnBuffAndDebuff(Card card)
        {

            Vector3 spawnPosition = effect.transform.position;
            // Instantiate the unit with the spawnOnGameObject as the parent object
            GameObject instantiatedObject = PhotonNetwork.Instantiate("Cards/Card Models/" + card.unitPrefab.name, spawnPosition, transform.rotation);

            instantiatedObject.transform.SetParent(effect);

            instantiatedObject.GetComponent<PhotonView>().TransferOwnership(parentObject.GetComponent<PhotonView>().Owner);
        }

    }

    private void ReturnCardAndDrawNew()
    {
      if (deck.hand.Count >  0)
     {
        // Check if the selected card can be returned
        Card selectedCard = deck.hand[selectedCardIndex];
        if (selectedCard.canBeReturned)
        {
            // Return the selected card to the deck
            deck.deck.Add(selectedCard);
            deck.hand.RemoveAt(selectedCardIndex);

            // Update selectedCardIndex to the new card index
            selectedCardIndex = deck.hand.Count -  1;

            // Draw a new card
            deck.DrawCard(1); // Draw one card
        }
        else
        {
            Debug.Log("This card cannot be returned.");
        }
      }
    }
}