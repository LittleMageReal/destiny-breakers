using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostField : MonoBehaviour
{
     private void Start()
    {
        //StartCoroutine(DestroyAfterDelay());
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            BuffAndDebuffManager buffAndDebuffManager = FindObjectOfType<BuffAndDebuffManager>();
            if (buffAndDebuffManager != null)
            {
               buffAndDebuffManager.SpawnFreezePrefab();
            }
        }
        
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
    
}
