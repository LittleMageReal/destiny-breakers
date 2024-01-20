using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomInputField;
    public TMP_InputField joinInput;
    public GameObject lobbyPanel;
    public GameObject roomPanel;

    List<int> playerIDs = new List<int>();

    private void Start()
    {
        PhotonNetwork.JoinLobby();
    }

    public void OnClickCreate()
    {
        if (roomInputField.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions() { MaxPlayers = 4 });
        }
    }

    public override void OnJoinedRoom()
    {
        playerIDs.Add(PhotonNetwork.LocalPlayer.ActorNumber);
        
        int[] playerIDArray = playerIDs.ToArray();
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
        {
            { "PlayerIDs", playerIDArray }
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public void StartGameButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SceneManager.LoadScene("Game");
        }
    }
}
