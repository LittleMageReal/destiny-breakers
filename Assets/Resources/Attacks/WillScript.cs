using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WillScript : MonoBehaviourPunCallbacks
{
    public int Will;
    public bool Rage = false;
    public int RageAmount;

    public bool illusion = false;

    public Health playerHealth;
    public bool Crush = false;
    public int CrushAmount;

    public bool Petrify = false;
    public int PetrifyAmount;

    private PhotonView photonView;
    private bool shouldDestroy = false;

    void Start()
    {
        playerHealth = GetComponentInParent<Health>();
        if (playerHealth == null)
        {
            Debug.Log("Health script not found in parent.");
        }

        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (Will < 0)
        {
            if (Crush)
            {
                playerHealth.LoseHealth(CrushAmount);
            }

            if (Petrify)
            {
                PetrifyAmount -= 1;
                Will = 500;
                if (PetrifyAmount <= 0)
                {
                    Petrify = false;
                }
                
            }

            if ((gameObject != null) && !Petrify)
            {
                shouldDestroy = true;
            }
        }

        if (shouldDestroy)
        {
            if (photonView.IsMine || PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }

        Crush = false;
    }

    public void TakeDamage(int amount)
    {
        Will -= amount;
        RageCheck();
        if (illusion)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void DealDamage(GameObject target)
    {
        var atm = target.GetComponent<WillScript>();
        if (atm != null)
        {
            atm.TakeDamage(Will);
        }
    }

    void RageCheck()
    {
        if (Rage == true)
        {
            Will += RageAmount;
        }
    }
}