using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBuff : MonoBehaviour
{
    public int willBoost = 200;
    private void Start()
    {
        StartCoroutine(DestroyAfterDelay());
    }
    void OnTriggerEnter(Collider other)
    {
        var willscript = other.gameObject.GetComponent<WillScript>();
        if (willscript != null)
        {
            willscript.Will += willBoost;
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(40f);
        Destroy(gameObject);
    }
}
