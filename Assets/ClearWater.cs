using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearWater : MonoBehaviour
{
    
    void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has a Deck script
        Deck deck = other.gameObject.GetComponent<Deck>();
        Spawn Spawner = other.gameObject.GetComponentInChildren<Spawn>();

        if (deck != null)
        {
            // Check if the player has any cards in their hand
            if (deck.hand.Count > 0)
            {
                // Move all cards from the hand to the deck
                foreach (Card card in deck.hand)
                {
                    // Check if the card is a Move card
                    if (card.Move)
                    {
                        // Destroy the card
                        Destroy(card);
                    }
                    else
                    {
                        // If the card is not a Move card, add it to the deck
                        deck.deck.Add(card);
                    }
                }

                // Clear the hand
                deck.hand.Clear();
            }
        }
    }
}
