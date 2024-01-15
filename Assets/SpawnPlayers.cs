using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public GameObject playerPrefab2;
    public GameObject playerPrefab3;
    public GameObject playerPrefab4;
    public GameObject spawnPoint1;
    public GameObject spawnPoint2;
    public GameObject spawnPoint3;
    public GameObject spawnPoint4;

    public CinemachineVirtualCamera vcam; // The Cinemachine Virtual Camera
    //public string LookAtName; // The name of the child object

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            GameObject player = null;

            // Check if this is the first player joining the room
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                // Spawn the first player at spawn point 1
                Vector3 spawnPosition = spawnPoint1.transform.position;
                Quaternion spawnRotation = spawnPoint1.transform.rotation;
                player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, spawnRotation);

                // Set the player transform in the CameraTarget script
                CameraTarget cameraTarget = player.GetComponent<CameraTarget>();
                if (cameraTarget != null)
                {
                    cameraTarget.player = player.transform;
                }
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                // Spawn the second player at spawn point 2
                Vector3 spawnPosition = spawnPoint2.transform.position;
                Quaternion spawnRotation = spawnPoint2.transform.rotation;
                player = PhotonNetwork.Instantiate(playerPrefab2.name, spawnPosition, spawnRotation);

                // Set the player transform in the CameraTarget script
                CameraTarget cameraTarget = player.GetComponent<CameraTarget>();
                if (cameraTarget != null)
                {
                    cameraTarget.player = player.transform;
                }
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == 3)
            {
                // Spawn the second player at spawn point 2
                Vector3 spawnPosition = spawnPoint3.transform.position;
                Quaternion spawnRotation = spawnPoint3.transform.rotation;
                player = PhotonNetwork.Instantiate(playerPrefab3.name, spawnPosition, spawnRotation);

                // Set the player transform in the CameraTarget script
                CameraTarget cameraTarget = player.GetComponent<CameraTarget>();
                if (cameraTarget != null)
                {
                    cameraTarget.player = player.transform;
                }
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == 4)
            {
                // Spawn the second player at spawn point 2
                Vector3 spawnPosition = spawnPoint3.transform.position;
                Quaternion spawnRotation = spawnPoint3.transform.rotation;
                player = PhotonNetwork.Instantiate(playerPrefab3.name, spawnPosition, spawnRotation);

                // Set the player transform in the CameraTarget script
                CameraTarget cameraTarget = player.GetComponent<CameraTarget>();
                if (cameraTarget != null)
                {
                    cameraTarget.player = player.transform;
                }
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