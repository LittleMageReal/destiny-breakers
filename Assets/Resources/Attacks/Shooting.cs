using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public WillScript willScript;

    public int ammo;

    public float bulletForce = 20f;
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
            if (Input.GetButtonDown("Fire1") && Time.time - lastUse >= Cooldown && ammo > 0)
            {
                lastUse = Time.time;
                ammo -= 1;
                Shoot();
            }
        }

    }

    void Shoot()
    {
        GameObject bullet = PhotonNetwork.Instantiate("Attacks/" + bulletPrefab.name, firePoint.position, firePoint.rotation);

        Damage damage = bullet.GetComponent<Damage>();
        damage.photonView.RPC("SetDamage", RpcTarget.All, willScript.Will);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode.Impulse);

        // Add a reference to the bullet's PhotonView in the Damage script
        damage.bulletPhotonView = bullet.GetComponent<PhotonView>();
    }
}