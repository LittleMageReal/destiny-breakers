using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WillUi : MonoBehaviour
{
    public WillScript willScript; // Reference to the WillScript
    public TMP_Text willText; // Reference to the UI Text object
    

    // Update is called once per frame
    void Update()
    {
        willText.text = willScript.Will.ToString();

    }
}
