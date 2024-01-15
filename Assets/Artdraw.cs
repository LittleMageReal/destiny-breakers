using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artdraw : MonoBehaviour
{
    public Deck deckScript;
    private int useCount = 3;
    private float scrollCooldown = 3f;
    private float lastUse;

    public void Start()
    {
       deckScript = GlobalReferens.instance.deckScript;
    }
    private void Update()
       
    {
        if (Input.GetKeyDown(KeyCode.Q) && Time.time - lastUse >= scrollCooldown) //cooldown 
        {
            lastUse = Time.time;
            deckScript.DrawCard(1);
            useCount--;

            if (useCount <= 0) // If the object has been used three times
            {
                Destroy(gameObject); // Destroy the object
            }
        }
        
    }
}
