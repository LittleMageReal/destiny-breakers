using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirages : MonoBehaviour
{
   public List<Card> Boon; // Assuming this is assigned in the inspector or through code

    void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has a Deck script
        Deck deck = other.gameObject.GetComponent<Deck>();

        if (deck!= null)
        {
            // Check if the player has any cards in their hand
            if (deck.hand.Count < 3)
            {
                // Select a random card from the Boon list
                Card randomCard = Boon[Random.Range(0, Boon.Count)];

                // Add the selected card to the player's hand
                deck.AddCardToHand(randomCard);

                // Optionally destroy the collider after adding a card
                Destroy(gameObject);
            }
        }
    }
}
