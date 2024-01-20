using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    public GameObject[] playerPrefabs;
    public GameObject[] spawnPoints;
    public CinemachineVirtualCamera vcam; // The Cinemachine Virtual Camera

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            GameObject player = null;

            int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber;

            // Check if the player index is within the range of available prefabs
            if (playerIndex >= 0 && playerIndex < playerPrefabs.Length)
            {
                // Spawn the player at the corresponding spawn point
                Vector3 spawnPosition = spawnPoints[playerIndex].transform.position;
                Quaternion spawnRotation = spawnPoints[playerIndex].transform.rotation;
                player = PhotonNetwork.Instantiate(playerPrefabs[playerIndex].name, spawnPosition, spawnRotation);

                PhotonView photonView = player.AddComponent<PhotonView>();
                ;

                // Set the player transform in the CameraTarget script
                CameraTarget cameraTarget = player.GetComponent<CameraTarget>();
                if (cameraTarget != null)
                {
                    cameraTarget.player = player.transform;
                }

                // Set the Cinemachine Virtual Camera to follow the player
                if (player != null)
                {
                    Transform childObject = player.transform;
                    if (childObject != null)
                    {
                        vcam.LookAt = childObject;
                    }
                    else
                    {
                        Debug.LogError("Child object not found");
                    }
                }
            }
        }
    }
}