using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ArkShoot : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public WillScript willScript;

    public float bulletForce = 20f;

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
            if (Input.GetButtonDown("Fire1"))
            {
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

        // Calculate the direction of the force
        Quaternion direction = Quaternion.Euler(new Vector3(90, 0, 90)); // Adjust the angle as needed

        // Apply the force in the calculated direction
        rb.AddForce(direction * firePoint.up * bulletForce, ForceMode.Impulse);
    }
}
