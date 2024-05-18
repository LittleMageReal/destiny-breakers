using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class CardUIManager : MonoBehaviour
{
    public Deck deck; // Reference to the Deck script
    public GameObject cardUIPrefab; // The Card UI prefab
    public Transform cardUIParent; // The parent object for the Card UIs
    public List<GameObject> cardUIs = new List<GameObject>(); // List to keep track of the UI objects
    private int lastSelectedIndex = -1; // Initialize to -1 to indicate no card is selected

    void Update()
    {
        // Check if the hand has changed and update the UI accordingly
        if (deck.hand.Count!= cardUIs.Count)
        {
            UpdateHandUI();
        }
    }

    void UpdateHandUI()
    {
        // Clear the existing UI objects
        foreach (GameObject cardUI in cardUIs)
        {
            Destroy(cardUI);
        }
        cardUIs.Clear();

        // Create UI for each card in the hand
        foreach (Card card in deck.hand)
        {
            CreateCardUI(card);
        }
    }

    private void CreateCardUI(Card card)
    {
        // Instantiate the card UI prefab
        GameObject cardUI = Instantiate(cardUIPrefab, cardUIParent);
        Card_Display cardUIScript = cardUI.GetComponent<Card_Display>();
        cardUIScript.Art.sprite = card.cardImage;
        cardUIScript.Cost.text = card.cardCost.ToString();
        cardUIScript.SetPanelColor(card.pointType);

        // Add the new Card UI object to the list
        cardUIs.Add(cardUI);
    }

    void OnEnable()
    {
        // Subscribe to the OnCardSelected event
        Spawn.OnCardSelected += HandleCardSelected;
    }

    void OnDisable()
    {
        // Unsubscribe from the OnCardSelected event
        Spawn.OnCardSelected -= HandleCardSelected;
    }

private void HandleCardSelected(int selectedIndex)
{
    // Check if the selected card index has changed
    if (selectedIndex!= lastSelectedIndex)
    {
        // If a card has been deselected, reset its scale and sorting order, then move it to the end of the sibling hierarchy
        if (lastSelectedIndex >= 0 && lastSelectedIndex < cardUIs.Count)
        {
            GameObject previouslySelectedCardUI = cardUIs[lastSelectedIndex];
            // Reset the scale of the previously selected card UI
            previouslySelectedCardUI.transform.localScale = Vector3.one; // Reset to original size
            // Reset the sorting order of the previously selected card UI
            Canvas canvas = previouslySelectedCardUI.GetComponent<Canvas>();
            if (canvas!= null)
            {
                canvas.sortingOrder = 1; // Reset sorting order to default or lowest value
            }
            // Move the previously selected card UI to the end of the sibling hierarchy
            previouslySelectedCardUI.transform.SetAsLastSibling();
        }

        // Update the lastSelectedIndex to the current selectedIndex
        lastSelectedIndex = selectedIndex;

        // If a new card is selected, scale the selected card UI
        if (selectedIndex >= 0 && selectedIndex < cardUIs.Count)
        {
            GameObject selectedCardUI = cardUIs[selectedIndex];
            // Scale the selected card UI
            selectedCardUI.transform.localScale = new Vector3(1.7f, 1.7f, 1f); // Scale factor
            // Center the selected card UI (implement based on your UI setup)
            Canvas canvas = selectedCardUI.GetComponent<Canvas>();
            if (canvas!= null)
            {
                canvas.sortingOrder = 2; // Set to a higher value to bring it forward
            }

            // Set the selected card UI as the first sibling of its content layer
            selectedCardUI.transform.SetParent(cardUIParent, false);
            selectedCardUI.transform.SetAsFirstSibling();
        }
    }
}

}