using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AShootgun : MonoBehaviour
{
    public Transform[] firePoints;
    public GameObject bulletPrefab;
    public WillScript willScript;

    public float bulletForce = 20f;
    public float spreadAngle = 10f; // Angle for bullet spread

    [SerializeField] private float Cooldown = 2f;
    private float lastUse;

    private new PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetButtonDown("Fire1") && Time.time - lastUse >= Cooldown)
            {
                lastUse = Time.time;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        foreach (Transform firePoint in firePoints)
        {
            GameObject bullet = PhotonNetwork.Instantiate("Attacks/" + bulletPrefab.name, firePoint.position, firePoint.rotation);

            Damage damage = bullet.GetComponent<Damage>();
            damage.photonView.RPC("SetDamage", RpcTarget.All, willScript.Will);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(firePoint.up * bulletForce, ForceMode.Impulse);
        }
    }
}
