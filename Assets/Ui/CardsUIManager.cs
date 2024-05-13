using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUIManager : MonoBehaviour
{
public Deck deck; // Reference to the Deck script
    public List<GameObject> cardUIs = new List<GameObject>(); // List of Card UI objects
    public TMP_Text cardEffect; // Text component for displaying card effects

    private int previousHandCount = 0; // Track the previous hand count to detect changes
    private int selectedCardIndex = 0; // Declare and initialize selectedCardIndex

    void Update()
    {
        // Check if the hand has changed
        if (deck.hand.Count!= previousHandCount)
        {
            // Calculate the difference in hand size
            int handSizeChange = deck.hand.Count - previousHandCount;

            // If the hand size has increased, add UI elements for new cards
            if (handSizeChange > 0)
            {
                for (int i = 0; i < handSizeChange; i++)
                {
                    Card cardToAdd = deck.hand[deck.hand.Count - 1 - i]; // Get the last added card
                    GameObject cardUI = InstantiateCardUI(cardToAdd);
                }
            }
            // If the hand size has decreased, remove UI elements for removed cards
            else if (handSizeChange < 0)
            {
                for (int i = 0; i < Mathf.Abs(handSizeChange); i++)
                {
                    GameObject cardUIToRemove = cardUIs[deck.hand.Count - 1 + i]; // Get the last removed card's UI
                    DestroyCardUI(cardUIToRemove);
                }
            }

            // Update the previous hand count
            previousHandCount = deck.hand.Count;
        }
    }

    // Method to instantiate and setup a card UI element
    public GameObject InstantiateCardUI(Card cardToAdd)
    {
        // Instantiate the card UI prefab
        GameObject cardUI = Instantiate(deck.cardUIPrefab, deck.cardUIParent);
        Card_Display cardUIScript = cardUI.GetComponent<Card_Display>();

        // Set the card image, cost, and color
        cardUIScript.Art.sprite = cardToAdd.cardImage;
        cardUIScript.Cost.text = cardToAdd.cardCost.ToString();
        cardUIScript.SetPanelColor(cardToAdd.pointType);

        // Add the new Card UI object to the cardUIs list
        cardUIs.Add(cardUI);

        return cardUI;
    }

    // Method to destroy a card UI object and remove it from the list
    public void DestroyCardUI(GameObject cardUI)
    {
        cardUIs.Remove(cardUI);
        Destroy(cardUI);
    }

    // Method to update the UI for a selected card
    public void UpdateCardUI(Card selectedCard)
    {
        if (selectedCard!= null)
        {
            cardEffect.text = selectedCard.cardEffect; // Update the card effect text

            // Scale the selected Card UI object
            for (int i = 0; i < cardUIs.Count; i++)
            {
                if (i == selectedCardIndex)
                {
                    cardUIs[i].transform.localScale = new Vector3(1.7f, 1.7f, 0f); // Scale up
                }
                else
                {
                    cardUIs[i].transform.localScale = new Vector3(1f, 1f, 1f); // Reset scale
                }
            }
        }
    }
}