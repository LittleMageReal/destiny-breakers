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
    public GameObject referenceObject; // The reference object used for alignment
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
            // If a card has been deselected, reset the UI for the previously selected card
            if (lastSelectedIndex >= 0 && lastSelectedIndex < cardUIs.Count)
            {
                GameObject previouslySelectedCardUI = cardUIs[lastSelectedIndex];
                // Reset the scale of the previously selected card UI
                previouslySelectedCardUI.transform.localScale = Vector3.one; // Reset to original size
                // Center the previously selected card UI (implement based on your UI setup)
                Canvas canvas = previouslySelectedCardUI.GetComponent<Canvas>();
                if (canvas!= null)
                {
                    canvas.sortingOrder = 1; // Reset sorting order
                }
            }

            // Update the lastSelectedIndex to the current selectedIndex
            lastSelectedIndex = selectedIndex;
        }

        // If a new card is selected, scale the selected card UI
        if (selectedIndex >= 0 && selectedIndex < cardUIs.Count)
        {
            GameObject selectedCardUI = cardUIs[selectedIndex];
            // Scale the selected card UI
            selectedCardUI.transform.localScale = new Vector3(1.7f, 1.7f, 1f); // Scale factor
            // Center the selected card UI based on the reference object's position
            Vector3 referencePosition = referenceObject.transform.position;
            Vector3 selectedCardPosition = selectedCardUI.transform.position;
            Vector3 difference = referencePosition - selectedCardPosition;
            Canvas canvas = selectedCardUI.GetComponent<Canvas>();
            if (canvas!= null)
            {
                canvas.sortingOrder = 2; // Set to a higher value to bring it forward
            }
            // Adjust the selected card's position to center it relative to the reference object
            Vector3 calculatedPosition = selectedCardUI.transform.parent.position + difference;

            // Start the coroutine to animate the position change
            StartCoroutine(AnimatePositionChange(selectedCardUI, calculatedPosition));
        }
    }

    IEnumerator AnimatePositionChange(GameObject selectedCardUI, Vector3 targetPosition)
    {
        // Store the initial position of the selected card UI
        Vector3 initialPosition = selectedCardUI.transform.parent.position;

        // Calculate the distance between the initial and target positions
        float distance = Vector3.Distance(initialPosition, targetPosition);

        // Calculate the duration of the animation based on the distance
        float animationDuration = distance / 1000f; // Adjust the divisor as needed for smoother or faster animation

        // Reset time for the animation
        float time = 0;

        while (time < animationDuration)
        {
            // Calculate the current position based on the interpolation of the initial and target positions
            Vector3 currentPosition = Vector3.Lerp(initialPosition, targetPosition, time / animationDuration);

            // Apply the current position to the selected card UI
            selectedCardUI.transform.parent.position = currentPosition;

            // Increment time
            time += Time.deltaTime;

            // Yield return null to wait for the next frame
            yield return null;
        }

        // Ensure the selected card UI reaches the target position
        selectedCardUI.transform.parent.position = targetPosition;
    }
}
