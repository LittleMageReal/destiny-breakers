using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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

    public List<GameObject> cardUIs = new List<GameObject>(); // List of Card UI objects
    public TMP_Text cardEffect;

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


    private void Start()
    {
        if (photonView.IsMine)
        {
            cardEffect = GameObject.Find("Effect").GetComponent<TMP_Text>();
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
                    cardEffect.text = selectedCard.cardEffect;
                }


                Debug.Log("number " + selectedCardIndex);

                // Scale the selected Card UI object
                for (int i = 0; i < cardUIs.Count; i++)
                {
                    if (i == selectedCardIndex)
                    {
                        cardUIs[i].transform.localScale = new Vector3(0.5f, 1.1f, 0f); // Scale up
                    }
                    else
                    {
                        cardUIs[i].transform.localScale = new Vector3(0.4f, 0.9f, 0f); // Reset scale
                    }
                }
            }



            if (Input.GetButtonDown("Fire2"))
            {
                if (deck.hand.Count > 0 && Time.time - rightMouseHoldStartTime < 3f) // 
                {
                    Card selectedCard = deck.hand[selectedCardIndex];

                    SpawnPrefab(deck.hand[selectedCardIndex]);
                }
            }

            if (Input.GetMouseButtonDown(1)) //  1 is the right mouse button
            {
              rightMouseHoldStartTime = Time.time;
              isRightMouseHeld = true;
            }
             else if (Input.GetMouseButtonUp(1)) //  1 is the right mouse button
            {
              if (isRightMouseHeld && Time.time - rightMouseHoldStartTime >=  3f)
               {
                  // Right mouse button was held for at least  3 seconds
                  ReturnCardAndDrawNew();
               }
              isRightMouseHeld = false;
            }
        }
        

    }
    public void SpawnPrefab(Card card)
    {

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

                     // Destroy the UI card object
                     Destroy(cardUIs[selectedCardIndex]);

                     // Remove the UI card from the list
                     cardUIs.RemoveAt(selectedCardIndex);
                    }
                else
                {
                 // Remove the card from the hand
                 deck.hand.Remove(card);

                 // Add the card back to the deck
                 deck.deck.Add(card);

                 // Destroy the UI card object
                 Destroy(cardUIs[selectedCardIndex]);

                 // Remove the UI card from the list
                 cardUIs.RemoveAt(selectedCardIndex);

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

            // Destroy the UI card object for the returned card
            Destroy(cardUIs[selectedCardIndex]);

            // Remove the UI card from the list for the returned card
            cardUIs.RemoveAt(selectedCardIndex);

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