using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HomingProjectile : MonoBehaviour
{
    [SerializeField] private Transform _target = null;
    [SerializeField] private GameObject[] targets;
    [SerializeField] private Rigidbody _rbMissile;
    [SerializeField] private float _distance;
    [SerializeField] private float _closestTarget = Mathf.Infinity;
    [SerializeField] private float _missileSpeed;
    [SerializeField] private float _missileTurnSpeed;
    //private new PhotonView photonView;

    void Start()
    {
        FindClosestEnemy();
    }

    void FixedUpdate()
    {
        FireMissile();
    }

    private void FindClosestEnemy()
    {
        targets = GameObject.FindGameObjectsWithTag("Player");

        foreach (var player in targets)
        {
            _distance = (player.transform.position - this.transform.position).sqrMagnitude;

            // Check if the current player is the one who instantiated the missile
            if (GetComponent<PhotonView>().IsMine && player.GetComponent<PhotonView>().IsMine)
            {
                continue; // Skip this player
            }

            if (_distance < _closestTarget)
            {
                _closestTarget = _distance;
                _target = player.transform;
            }
        }
    }

    private void FireMissile()
    {
        _rbMissile.velocity = transform.up * _missileSpeed * Time.deltaTime;

        if (_target != null)
        {
            Vector3 direction = _target.position - _rbMissile.position;
            direction.Normalize();

            Vector3 crossProduct = Vector3.Cross(direction, transform.up);
            _rbMissile.angularVelocity = new Vector3(-crossProduct.x * _missileTurnSpeed, -crossProduct.y * _missileTurnSpeed, -crossProduct.z * _missileTurnSpeed);
        }
    }

    //void OnCollisionEnter(Collision collision)
   // {
      // if (collision.gameObject == _target.gameObject)
      // {
         //  PhotonNetwork.Destroy(this.gameObject);
      // }
   // }
}