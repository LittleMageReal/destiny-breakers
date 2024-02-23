using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Deck : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public List<Card> hand = new List<Card>();

    public GameObject cardUIPrefab; // The Card UI Prefab
    public Transform cardUIParent; // The parent transform for the Card UI Prefabs

    public bool FreezeDeck = false;

    private new PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
            cardUIParent = GameObject.Find("Content").GetComponent<RectTransform>();
            ShuffleDeck();
            DrawCard(0);
        }
    }

    

    void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    public void DrawCard(int numberOfCards)
    {
       for (int i = 0; i < numberOfCards; i++)
        {
            // Check if the hand is already full
            if (hand.Count >= 3 && FreezeDeck == false) //hand limit 
            {
                break; 
            }

            if (deck.Count > 0 && FreezeDeck == false)
            {
                Card drawnCard = deck[0];
                deck.RemoveAt(0);
                hand.Add(drawnCard);

                if (photonView.IsMine)
                {
                    // Create a new Card UI Prefab for the drawn card
                    GameObject cardUI = Instantiate(cardUIPrefab, cardUIParent);
                    Card_Display cardUIScript = cardUI.GetComponent<Card_Display>();
                    cardUIScript.Art.sprite = drawnCard.cardImage;
                    cardUIScript.Cost.text = drawnCard.cardCost.ToString();
                    cardUIScript.SetPanelColor(drawnCard.pointType);

                    // Add the new Card UI object to the cardUIs list
                    Spawn.instance.cardUIs.Add(cardUI);
                }
               
            }
        }
    }

    public void ReturnCard(int numberOfCards)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            // Check if the hand is already full
            if (hand.Count >= 3) //hand limit 
            {
                break;
            }

            if (deck.Count > 0)
            {
                Card drawnCard = deck[deck.Count - 1]; // Get the last card in the deck
                deck.RemoveAt(deck.Count - 1); // Remove the last card from the deck
                hand.Add(drawnCard);

                // Create a new Card UI Prefab for the drawn card
                GameObject cardUI = Instantiate(cardUIPrefab, cardUIParent);
                Card_Display cardUIScript = cardUI.GetComponent<Card_Display>();
                cardUIScript.Art.sprite = drawnCard.cardImage;
                cardUIScript.Cost.text = drawnCard.cardCost.ToString();
                cardUIScript.SetPanelColor(drawnCard.pointType);

                // Add the new Card UI object to the cardUIs list
                Spawn.instance.cardUIs.Add(cardUI);
            }
        }
    }



    void OnTriggerEnter(Collider other)
    {
        // Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (other.gameObject.tag == "Finish")
        {
            // Draw a full hand of cards
            DrawCard(3);
        }
    }

}
