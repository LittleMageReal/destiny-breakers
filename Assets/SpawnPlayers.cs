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
        // Get the player's custom properties to determine which prefab to instantiate
        int playerAvatarIndex = (int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"];
        GameObject playerPrefab = playerPrefabs[playerAvatarIndex];

        // Choose a random spawn point
        int randomNumber = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomNumber].transform;

        // Instantiate the player prefab at the chosen spawn point
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);

                

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