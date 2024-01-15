using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomSpawn : MonoBehaviour
{
    public Rigidbody s;
    public Vector3 minDirection = new Vector3(-1f, -1f, -1f);
    public Vector3 maxDirection = new Vector3(1f, 1f, 1f);
    public int minObjects = 1;
    public int maxObjects = 3;
    

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnObjects", 0f, 3f);
    }

    void SpawnObjects()
    {
        int numberOfObjects = Random.Range(minObjects, maxObjects + 1);
        for (int i = 0; i < numberOfObjects; i++)
        {
            Rigidbody clone;
            clone = Instantiate(s, transform.position, transform.rotation);
            Vector3 randomDirection = Vector3.Lerp(minDirection, maxDirection, Random.Range(0f, 1f));
            clone.velocity = randomDirection * 10;
        }
    }
}
