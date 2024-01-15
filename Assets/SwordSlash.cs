using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SwordSlash : MonoBehaviour
{
    public GameObject attackPrefab;
    public Transform spawnPoint;
    public WillScript willScript;
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
        GameObject attackInstance = PhotonNetwork.Instantiate("Attacks/" + attackPrefab.name, spawnPoint.position, Quaternion.Euler(0, 0, 0));

        Damage damage = attackInstance.GetComponent<Damage>();
        damage.photonView.RPC("SetDamage", RpcTarget.All, willScript.Will);

        StartCoroutine(DestroyAfterDelay(attackInstance, 0.3f));  // delay the destruction of attackInstance
    }

    private IEnumerator DestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(0.3f);
        PhotonNetwork.Destroy(obj);

        if (useCount <= 0) // If the object has been used three times
        {
            PhotonNetwork.Destroy(gameObject); // Destroy the object
        }
    }
}
