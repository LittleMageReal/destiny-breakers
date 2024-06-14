using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class inputHandler : MonoBehaviourPunCallbacks
{
    [SerializeField] private KArtController kartController;
    private PhotonView photonView;
    PhotonView View;

    private void Start()
    {
        View = GetComponentInParent<PhotonView>();
    }

    private void Update()
    {
        if (View.IsMine)
        {
            kartController._forwardAmount = Input.GetAxis("Vertical");
            kartController._turnAmount = Input.GetAxis("Horizontal");

            if (kartController._forwardAmount!= 0)
                kartController.Drive();
            else
                kartController.Stand();

            kartController.TurnHandler();

            if (Input.GetButtonDown("Jump") &&!kartController._isDrifting && Mathf.Abs(kartController._turnAmount) >= 0.5)
                kartController.StartDrift();

            if (kartController._isDrifting && (Input.GetButtonUp("Jump") || (Input.GetKeyUp(KeyCode.W))))
                kartController.EndDrift();

            if (Input.GetKeyDown(KeyCode.LeftShift) &&!kartController.isSpeedBoostActive)
            {
                kartController.ActivateSpeedBoost();
            }

            if (kartController.speedBoostCooldown > 0)
            {
                kartController.speedBoostCooldown -= Time.deltaTime;
            }
        }
    }
}
