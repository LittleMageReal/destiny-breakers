using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FairyShoot : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public WillScript willScript;

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
            if (Input.GetButtonDown("Fire1") && Time.time - lastUse >= Cooldown)
            {
                lastUse = Time.time;
                StartCoroutine(Shoot());
            }
        }

    }

    IEnumerator Shoot()
    {
        int numberOfBullets = 5;
        float delay = 0.1f;

        for (int i = 0; i < numberOfBullets; i++)
        {
            GameObject bullet = PhotonNetwork.Instantiate("Attacks/" + bulletPrefab.name, firePoint.position, firePoint.rotation);

            Damage damage = bullet.GetComponent<Damage>();
            damage.photonView.RPC("SetDamage", RpcTarget.All, willScript.Will);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(firePoint.up * bulletForce, ForceMode.Impulse);

            yield return new WaitForSeconds(delay);
        }
        
    }
}
