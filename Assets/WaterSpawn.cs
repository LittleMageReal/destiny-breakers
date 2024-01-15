using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WaterSpawn : MonoBehaviour
{
    public GameObject WaterPrefab;
    void OnCollisionEnter(Collision collision)
    {
        PhotonNetwork.Destroy(gameObject);
        PhotonNetwork.Instantiate("Attacks/refreshtide", transform.position, Quaternion.Euler(0, 90, 0));
    }
}
