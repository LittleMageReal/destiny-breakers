using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : MonoBehaviour
{
    public int damageAmount = 100;
    private void Start()
    {
        StartCoroutine(DestroyAfterDelay());
    }
    void OnTriggerEnter(Collider other)
    {
        var healthScript = other.gameObject.GetComponent<Health>();
        if (healthScript != null)
        {
            healthScript.LoseHealth(damageAmount);
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
}
