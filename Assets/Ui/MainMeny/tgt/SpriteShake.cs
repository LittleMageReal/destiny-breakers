using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShake : MonoBehaviour
{
    public float shakeDuration = 0.1f; // Duration of the shake effect in seconds
    public float shakeMagnitude = 0.05f; // Magnitude of the shake effect
    public float shakeInterval = 2f; // Time interval between shakes in seconds

    private Vector3 originalPosition;

    void Start()
    {
        // Store the original position of the sprite
        originalPosition = transform.localPosition;

        // Start the shake coroutine
        StartCoroutine(ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine()
    {
        while (true)
        {
            // Wait for the shake interval
            yield return new WaitForSeconds(shakeInterval);

            // Calculate the new position with a random offset
            Vector3 newPosition = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            transform.localPosition = newPosition;

            // Wait for the shake duration
            yield return new WaitForSeconds(shakeDuration);

            // Return the sprite to its original position
            transform.localPosition = originalPosition;
        }
    }
}
