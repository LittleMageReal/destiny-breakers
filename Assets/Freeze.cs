using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour
{

    private void Update()
    {
        var deck = GetComponentInParent<Deck>();
        if (deck != null)
        {
            deck.FreezeDeck = true; // Set the FreezeDeck property of the Deck class to true
        }
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(7f);
        var deck = GetComponentInParent<Deck>();
        if (deck != null)
        {
            deck.FreezeDeck = false; // Set the FreezeDeck property of the Deck class to true
        }
        Destroy(gameObject);
    }
}
