using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class StartGame : MonoBehaviour
{
    public void StartGameButtonClicked()
    {
       SceneManager.LoadScene("Game");
    }
}
