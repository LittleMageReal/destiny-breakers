using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiAnimations : MonoBehaviour
{
    public GameObject TextPrefab;
    public Transform SpawnPoint;
    public Transform SpawnPoint2;
    public Transform SpawnPoint3;
    public float MoveDuration = 5f; // Duration for moving the text to the left
    public float SpawnInterval = 4f; // Interval between spawning text prefabs

    void Start()
    {
        StartCoroutine(SpawnAndMoveText());
    }

    IEnumerator SpawnAndMoveText()
    {
        while (true)
        {
            yield return new WaitForSeconds(SpawnInterval);

            // Spawn the text prefab
            GameObject textInstance = Instantiate(TextPrefab, SpawnPoint);

            // Move the text to the left
            yield return StartCoroutine(MoveTextToLeft(textInstance, MoveDuration));

            // Destroy the text prefab after moving
            Destroy(textInstance);

            // Wait for before spawning the next text prefab
            yield return new WaitForSeconds(SpawnInterval);

            // Spawn the text prefab
            GameObject textInstance2 = Instantiate(TextPrefab, SpawnPoint2);

            // Move the text to the left
            yield return StartCoroutine(MoveTextToUp(textInstance2, MoveDuration));

            // Destroy the text prefab after moving
            Destroy(textInstance2);

            // Wait for before spawning the next text prefab
            yield return new WaitForSeconds(SpawnInterval);

             // Spawn the text prefab
            GameObject textInstance3 = Instantiate(TextPrefab, SpawnPoint3);

            // Move the text to the left
            yield return StartCoroutine(MoveTextToDown(textInstance3, MoveDuration));

            // Destroy the text prefab after moving
            Destroy(textInstance3);

            // Wait for before spawning the next text prefab
            yield return new WaitForSeconds(SpawnInterval);
        }
    }

    IEnumerator MoveTextToLeft(GameObject textInstance, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = textInstance.transform.position;
        Vector3 endPosition = startPosition + new Vector3(-4000f, 0, 0); //final Position

        while (elapsedTime < duration)
        {
            textInstance.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textInstance.transform.position = endPosition;
    }

    IEnumerator MoveTextToUp(GameObject textInstance, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = textInstance.transform.position;
        Vector3 endPosition = startPosition + new Vector3(3683f, 3382f, 0); //final Position

        while (elapsedTime < duration)
        {
            textInstance.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textInstance.transform.position = endPosition;
    }

    
    IEnumerator MoveTextToDown(GameObject textInstance, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = textInstance.transform.position;
        Vector3 endPosition = startPosition + new Vector3(3683f,-3382f, 0); //final Position

        while (elapsedTime < duration)
        {
            textInstance.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textInstance.transform.position = endPosition;
    }
}
