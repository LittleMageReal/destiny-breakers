using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class StoryChose : MonoBehaviour
{
    public void OnClickConnect()
    {
        SceneManager.LoadScene("Solo");
    }
}
