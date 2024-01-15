using UnityEngine;
using Photon.Pun;

public class Damage : MonoBehaviour
{
    public int damageAmount = 10; // Set this to the amount of damage you want the bullet to do
    public bool Punish;
    [SerializeField] private float DestroctionTime = 0.1f;
    [SerializeField] private bool Destroeble;
    [SerializeField] private bool isDestroyed;
    public PhotonView bulletPhotonView;

    public PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    void OnCollisionEnter(Collision collision)
    {
        var willScript = collision.gameObject.GetComponent<WillScript>();
        if (willScript != null)
        {
            willScript.TakeDamage(damageAmount);
        }

        if (Punish == true)
        {
            var health = collision.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.LoseHealth(damageAmount);
            }
        }
        if (photonView.IsMine || Destroeble == true)
        {
            Invoke("DestroyBullet", DestroctionTime);
        }
        
    }
     void DestroyBullet()
     {
        if (photonView.IsMine)
        {
            if (!isDestroyed && gameObject != null)
            {
                PhotonNetwork.Destroy(gameObject);
                isDestroyed = true;
            }
        }
        
    }

    [PunRPC]
    public void SetDamage(int damage)
    {
        damageAmount = damage;
    }
}