using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class spawnShield : MonoBehaviour
{
    public GameObject ShieldPrefab;
    public Transform spawnPoint;
    [SerializeField] private int useCount = 3;
    private float Cooldown = 3f;
    private float lastUse;

    private new PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Q) && Time.time - lastUse >= Cooldown)
            {
                lastUse = Time.time;
                Invoke("Attack", 0.1f);  // waits for 0.4 seconds before calling Attack
                useCount--;
            }
        }
        
    }

    public void Attack()
    {
        GameObject attackInstance = PhotonNetwork.Instantiate("Attacks/" + ShieldPrefab.name, spawnPoint.position, Quaternion.Euler(0, 0, 0));
        // Set the parent of the instantiated shield to the spawn point
        attackInstance.transform.SetParent(spawnPoint, true);

        // Optionally, reset the local position and rotation of the shield to ensure it's correctly positioned relative to the spawn point
        attackInstance.transform.localPosition = Vector3.zero;
        attackInstance.transform.localRotation = Quaternion.identity;

        StartCoroutine(DestroyAfterDelay(attackInstance, 3f));  // delay the destruction of attackInstance
    }

    private IEnumerator DestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(3f);
        PhotonNetwork.Destroy(obj);

        if (useCount <= 0) // If the object has been used three times
        {
            PhotonNetwork.Destroy(gameObject); // Destroy the object
        }
    }
}
