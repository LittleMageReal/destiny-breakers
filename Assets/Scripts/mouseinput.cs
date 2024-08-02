using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class mouseinput : MonoBehaviour
{
    public Spawn spawnscript;

    private float rightMouseHoldStartTime =  0f; // Time when the right mouse button was first pressed
    private bool isRightMouseHeld = false; // Whether the right mouse button is currently being held down

    private new PhotonView photonView;

    // Start is called before the first frame update
    void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
      if (photonView.IsMine)
        {
          float scroll = Input.GetAxis("Mouse ScrollWheel");
          spawnscript.Scrolling(scroll);
        

         // Card summoning and returning cards to deck
         if (Input.GetButtonDown("Fire2")) 
            {
                isRightMouseHeld = true;

                rightMouseHoldStartTime = Time.time;
            }

         if (Input.GetButtonUp("Fire2"))
            {
                if (Time.time - rightMouseHoldStartTime >= 3f)
                {
                    spawnscript.ReturnCardAndDrawNew();
                }
                else
                {
                    Card selectedCard = spawnscript.deck.hand[spawnscript.selectedCardIndex];
                    spawnscript.SpawnPrefab(selectedCard);
                }

                isRightMouseHeld = false;
            }
        }
    }
}
