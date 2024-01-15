using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoneWolf : MonoBehaviour
{
    public Deck deck;
    public WillScript willScript;

    private int previousHandCount;

    void Start()
    {
        deck = GetComponentInParent<Deck>();

        int missingCards = 3 - deck.hand.Count;
        if (missingCards < 0) missingCards = 0;

        // Add 100 Will for each missing card
        willScript.Will += missingCards * 100;

        previousHandCount = deck.hand.Count;
    }

    // Update is called once per frame
    void Update()
    {
        int currentHandCount = deck.hand.Count;

        // If the hand count has decreased
        if (currentHandCount > previousHandCount)
        {
            // Subtract 100 Will for each card removed
            willScript.Will += (previousHandCount - currentHandCount) * 100;
        }
        // If the hand count has increased
        else if (currentHandCount < previousHandCount)
        {
            // Add 100 Will for each card added
            willScript.Will -= (currentHandCount - previousHandCount) * 100;
        }

        // Update the previous hand count
        previousHandCount = currentHandCount;

    }
}
