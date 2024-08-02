using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionTimer : MonoBehaviour
{
    public float delay = 0f;
    public bool ass = true;
    public int deer = 75;

    private void Start()
    {
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
