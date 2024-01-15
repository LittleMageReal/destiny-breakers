using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Phoenix : MonoBehaviour
{
    public int healAmount = 100; // The amount of HP to heal
    public Deck deckScript;

    void Start()
    {
        // Find the Health component in the scene
        Health health = GetComponentInParent<Health>();
        deckScript = GetComponentInParent<Deck>();

        // Call the GainHealth method
        if (health != null)
        {
            health.GainHealth(healAmount);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1")) //cooldown 
        {
            PhotonNetwork.Destroy(gameObject); 
            deckScript.ReturnCard(1);
 
        }
    }
}
