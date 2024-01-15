using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownOnCollision : MonoBehaviour
{
    public float slowDownFactor = 0.5f; // Control how much the player should be slowed down
    public float scriptDisableDuration = 1f; // Control how long the player's script should be disabled

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Sphere"))
        {
            Rigidbody playerRb = other.gameObject.GetComponent<Rigidbody>();
            playerRb.velocity *= slowDownFactor;
        }

    }

}

