using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BossBehave : MonoBehaviour
{
    public GameObject bulletPrefab; // Assign your bullet prefab in the inspector
    public float speed = 90f;
    [SerializeField] private int Attak;
    public Transform[] firePoints;

    public float bulletForce = 20f;
    private new PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        StartCoroutine(SpawnBullets());
    }

    IEnumerator SpawnBullets()
    {
        // Initial delay
        yield return new WaitForSeconds(7);
        // Calculate end time
        float endTime = Time.time + 3;

        // Start time
        float startTime = Time.time;

        while (Time.time < endTime) // Continue for 3 seconds
        {
            // Rotate the boss
            transform.rotation = Quaternion.Euler(0, speed * Time.deltaTime, 0);

            foreach (Transform firePoint in firePoints)
            {
                GameObject bullet = PhotonNetwork.Instantiate("Attacks/" + bulletPrefab.name, firePoint.position, firePoint.rotation);

                Damage damage = bullet.GetComponent<Damage>();
                damage.photonView.RPC("SetDamage", RpcTarget.All, Attak);

                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                rb.AddForce(firePoint.up * bulletForce, ForceMode.Impulse);
            }

            // Wait for 1 frame before continuing the loop
            yield return null;
        }
    }
}
