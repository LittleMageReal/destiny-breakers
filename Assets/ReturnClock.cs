using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ReturnClock : MonoBehaviour
{
    public Deck deckScript;
    [SerializeField] private int useCount = 3;
    private float scrollCooldown = 3f;
    private float lastUse;

    private new PhotonView photonView;
    public void Start()
    {
        deckScript = GetComponentInParent<Deck>();
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Q) && Time.time - lastUse >= scrollCooldown) //cooldown 
            {
                lastUse = Time.time;
                deckScript.ReturnCard(1);
                useCount--;

                if (useCount <= 0) // If the object has been used three times
                {
                    PhotonNetwork.Destroy(gameObject); // Destroy the object
                }
            }
        }

    }
}
