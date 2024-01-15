using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCartToHand : MonoBehaviour
{
    public Card cardToAddWhenSummoned;

    private Deck deck;
    private GameObject cardUI;

    void Start()
    {
        // Find the deck in the parent object
        deck = GetComponentInParent<Deck>();

        // If the deck is found and the card to add is not null, add the card to the hand
        if (deck != null && cardToAddWhenSummoned != null)
        {
            deck.hand.Add(cardToAddWhenSummoned);

            // Create a new Card UI Prefab for the drawn card
            cardUI = Instantiate(deck.cardUIPrefab, deck.cardUIParent);
            Card_Display cardUIScript = cardUI.GetComponent<Card_Display>();
            cardUIScript.Art.sprite = cardToAddWhenSummoned.cardImage;
            cardUIScript.Cost.text = cardToAddWhenSummoned.cardCost.ToString();
            cardUIScript.SetPanelColor(cardToAddWhenSummoned.pointType);

            // Add the new Card UI object to the cardUIs list
            Spawn.instance.cardUIs.Add(cardUI);
        }
    }

    void OnDestroy()
    {
        // If the unit is destroyed, remove the card from the hand
        if (deck != null && cardToAddWhenSummoned != null)
        {
            deck.hand.Remove(cardToAddWhenSummoned);

            // Remove the Card UI object from the cardUIs list and destroy it
            if (cardUI != null)
            {
                Spawn.instance.cardUIs.Remove(cardUI);
                Destroy(cardUI);
            }
        }
    }
}
