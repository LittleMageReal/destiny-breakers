using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class HealthUI : MonoBehaviourPunCallbacks
{
    public Health HealthScript; // Reference to the HealthScript
    public TMP_Text Health; // Reference to the UI Text object

    private new PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
            // Find the "hp" text object and assign it to the Health variable
            Health = GameObject.Find("Hp").GetComponent<TMP_Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            Health.text = HealthScript.Hp.ToString();
        }
    }
}