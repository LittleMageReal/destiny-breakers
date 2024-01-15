using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenSpawn : MonoBehaviour
{
    public GameObject SKelly;


    void Start()
    {
        StartCoroutine(SpawnRoutine());
        Destroy(gameObject, 9f);
    }
    
    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            GameObject spawnedSkelly = Instantiate(SKelly, transform.position, transform.rotation);
            Destroy(spawnedSkelly, 20f);
            yield return new WaitForSeconds(3);
        }
    }
}
