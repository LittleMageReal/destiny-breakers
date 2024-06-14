using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MenyController : MonoBehaviour
{
    public GameObject Online;
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Online.SetActive(false);
        }
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void OnlineUi()
    {
        Online.SetActive(true);
    }

    public void OnClickConnect()
    {
        SceneManager.LoadScene("StoryChose");
    }
}
