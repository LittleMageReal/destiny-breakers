using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReferens : MonoBehaviour
{
    public static GlobalReferens instance;

    public Deck deckScript;

    void Awake()
    {
        instance = this;
    }
}
