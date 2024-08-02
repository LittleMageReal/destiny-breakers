using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BotSpawn : MonoBehaviourPunCallbacks
{
    public GameObject SKelly;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameObject spawnedSkelly = PhotonNetwork.Instantiate(SKelly.name, transform.position, transform.rotation);
        }
    }
}
