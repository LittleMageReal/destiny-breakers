using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Drown : MonoBehaviour
{
    public float shrinkTime = 1f; // Time in seconds for the object to shrink
    void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has a "SpawnPoint" child
        Transform spawnPoint = other.transform.Find("SpawnPoint");
        if (spawnPoint != null)
        {
            // If it does, destroy all of its children
            foreach (Transform child in spawnPoint)
            {
                PhotonNetwork.Destroy(child.gameObject);
                // Start shrinking the object that this script is attached to
                StartCoroutine(ShrinkAndDestroy(this.gameObject));
            }
        }
    }

    IEnumerator ShrinkAndDestroy(GameObject target)
    {
        float elapsedTime = 0f;

        while (elapsedTime < shrinkTime)
        {
            // Calculate the new scale
            Vector3 newScale = Vector3.Lerp(target.transform.localScale, Vector3.zero, elapsedTime / shrinkTime);

            // Apply the new scale
            target.transform.localScale = newScale;

            // Wait for the next frame
            yield return null;

            // Increase the elapsed time
            elapsedTime += Time.deltaTime;
        }
        // Destroy the object
        PhotonNetwork.Destroy(target);
    }
}