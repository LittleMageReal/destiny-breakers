using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPoint : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DestroySelf());
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            DriftPointManager.Instance.AddPoints(Card.PointType.Blue);
            Destroy(gameObject);
        }
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(7);
        Destroy(gameObject);
    }
}
