using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BossBehave : MonoBehaviour
{
    public GameObject bulletPrefab; // Assign your bullet prefab in the inspector
    public float speed = 90f; // Speed at which the boss rotates
    [SerializeField] private int Attack; // Corrected typo here
    public Transform[] firePoints;
    public float amount = 2;

    public float bulletForce = 20f;

    public int CurentPowah;
    public bool shouldContinueAttacking = false;
    public WillScript power;
    private new PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        Awoken();
    }

    private void Update()
    { 
        if(CurentPowah != power.Will && !shouldContinueAttacking)
        {
            Awoken();
            CurentPowah = power.Will;
        }
        else
        {
            IsDormant();
        }
    }

    IEnumerator SpawnBullets()
    {
        // Initial delay
        yield return new WaitForSeconds(7);
        
        // Calculate rotation speed for a full circle in 2 seconds
        float rotationSpeed = 360f / 2; // Degrees per second
        
        // Start time
        float startTime = Time.time;

        while (shouldContinueAttacking) // Infinite loop for continuous behavior
        {
            // Rotate the boss
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            // Check if 2 seconds have passed since the last bullet was shot
            if (Time.time >= startTime + amount)
            {
                // Reset start time for next bullet
                startTime = Time.time;

                foreach (Transform firePoint in firePoints)
                {
                    GameObject bullet = PhotonNetwork.Instantiate("Attacks/" + bulletPrefab.name, firePoint.position, firePoint.rotation);

                    Damage damage = bullet.GetComponent<Damage>();
                    damage.photonView.RPC("SetDamage", RpcTarget.All, Attack); // Ensure 'Attack' is correctly spelled

                    Rigidbody rb = bullet.GetComponent<Rigidbody>();
                    rb.AddForce(firePoint.up * bulletForce, ForceMode.Impulse);
                }
            }

            // Wait for 1 frame before continuing the loop
            yield return null;
        }
    }

    public void StopAttacking()
{
    {
     shouldContinueAttacking = false;
    }
}
   public void Awoken(){
        shouldContinueAttacking = true;
        StartCoroutine(SpawnBullets());
   }

  void IsDormant()
  {
    CurentPowah = power.Will;
    switch(CurentPowah)
    {
     case 0:
      StopAttacking();
      break;
    }
  }

}
