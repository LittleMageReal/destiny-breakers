using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Deck : MonoBehaviour
{
    public List<Card> deck;
    public List<Card> hand = new List<Card>();
    public bool FreezeDeck = false;

    private new PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
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

    public void AddCardToHand(Card card)
    {
       hand.Add(card);
    }

}
