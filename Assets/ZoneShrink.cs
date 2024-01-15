using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneShrink : MonoBehaviour
{
    public float shrinkInterval = 5f;
    public float shrinkRate = 0.1f;
    public float minSize = 1f;
    public int damageAmount = 10;
    public float damageInterval = 1f; // Time between each damage tick
    

    private void Start()
    {
        StartCoroutine(ShrinkZone());
        StartCoroutine(DealDamage()); // Start the damage coroutine
    }


    private IEnumerator ShrinkZone()
    {
        while (true)
        {
            yield return new WaitForSeconds(shrinkInterval);
            if (transform.localScale.x > minSize && transform.localScale.z > minSize)
            {
                transform.localScale -= new Vector3(shrinkRate, 0, shrinkRate);
            }
        }
    }

    private IEnumerator DealDamage() // New coroutine for dealing damage
    {
        while (true)
        {
            yield return new WaitForSeconds(damageInterval);
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); // Find all players
            foreach (var player in players)
            {
                if (!IsInsideZone(player.transform.position))
                {
                    // Get the Health component and call LoseHealth
                    var healthScript = player.GetComponent<Health>();
                    if (healthScript != null)
                    {
                        healthScript.LoseHealth(damageAmount);
                    }
                }
            }
        }
    }

    private bool IsInsideZone(Vector3 position)
    {
        return Vector3.Distance(position, transform.position) <= transform.localScale.x / 2;
    }
}
