using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class heathtrigger : MonoBehaviour
{
    public int healAmount = 400; // The amount of HP to heal
    public Health playerHealth;
    private new PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        playerHealth = GetComponentInParent<Health>(); // Correctly find the Health component
        // Subscribe to the OnHealthLost event using the class name
        Health.OnHealthLost += ReactToHealthLoss;
    }

    void OnDestroy()
    {
        // Unsubscribe from the OnHealthLost event when the reactor is destroyed
        Health.OnHealthLost -= ReactToHealthLoss;
    }

    void ReactToHealthLoss()
    {
        if (photonView.IsMine)
        {
            // Increase the player's health
         playerHealth.GainHealth(healAmount);

         // Destroy this object after reacting
         PhotonNetwork.Destroy(gameObject);

        }
    }
}
