using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessFish : MonoBehaviour
{
    [SerializeField] private Card Fish;
    void Start()
    {
        var water = GetComponentInParent<Deck>();
        if (water != null)
        {
           water.deck.Add(Fish);
           water.deck.Add(Fish);
        }
        
    }

}
