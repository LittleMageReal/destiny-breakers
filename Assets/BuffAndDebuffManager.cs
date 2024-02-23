using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffAndDebuffManager : MonoBehaviour
{

    public GameObject freezePrefab;

    
    public void SpawnFreezePrefab()
    {
        Instantiate(freezePrefab, transform);
    }
}
