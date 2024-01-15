using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crush : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        WillScript willScript = collision.gameObject.GetComponent<WillScript>();
        if (willScript != null)
        {
            willScript.Crush = true;
        }
    }
}
