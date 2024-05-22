using UnityEngine;
using Photon.Pun;
using Photon.Realtime; // Add this line

public class SoloSpawn : MonoBehaviourPunCallbacks
{
    void Start()
    {
        // Enable offline mode
        PhotonNetwork.OfflineMode = true;
        Debug.Log("Offline mode enabled");

        // Manually create a room with specific settings
        PhotonNetwork.CreateRoom("TestRoom", new RoomOptions(), null);
        Debug.Log("Room created");
    }
}