using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChargedAttack : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public WillScript willScript;

    public float maxHoldDuration = 3f; // Maximum duration to consider as "holding"
    private float buttonPressStartTime = 0f; // Time when the button was first pressed
    private Coroutine chargeCoroutine;

    public float bulletForce = 20f;

    private new PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            buttonPressStartTime = Time.time;
            chargeCoroutine = StartCoroutine(ChargeAfterDelay());
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if (Time.time - buttonPressStartTime < maxHoldDuration)
            {
                StopCoroutine(chargeCoroutine);
                Shoot();
            }
        }
    }
    // Start counting up to 3 when button is held
    IEnumerator ChargeAfterDelay()
    {
        yield return new WaitForSeconds(maxHoldDuration);
        Charge();
    }

    void Charge()
    {
        // Double the will
        willScript.Will += 200;
    }

    void Shoot()
    {
        GameObject bullet = PhotonNetwork.Instantiate("Attacks/" + bulletPrefab.name, firePoint.position, firePoint.rotation);

        Damage damage = bullet.GetComponent<Damage>();
        damage.photonView.RPC("SetDamage", RpcTarget.All, willScript.Will);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode.Impulse);
    }
}
