using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aibotspawner : MonoBehaviour
{
    public Deck deck; // Reference to the deck
    public Spawn spawnScript; // Reference to the Spawn script
    public float summonInterval = 5f; // Time interval between summons in seconds

    private float nextSummonTime; // Time when the next summon can occur

    void Start()
    {
        // Initialize the next summon time
        nextSummonTime = Time.time + summonInterval;
    }

    void Update()
    {
        // Check if it's time to summon a unit
        if (Time.time >= nextSummonTime && deck.hand.Count > 0)
        {
            SummonUnit();
            // Schedule the next summon
            nextSummonTime = Time.time + summonInterval;
        }
    }

    void SummonUnit()
    {
        // Select a random card from the deck
        int randomIndex = UnityEngine.Random.Range(0, deck.hand.Count);
        Card selectedCard = deck.hand[randomIndex];

        // Summon the unit corresponding to the selected card
        spawnScript.SpawnPrefab(selectedCard);
    }
}
