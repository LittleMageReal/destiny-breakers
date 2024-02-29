using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KArtController : MonoBehaviour
{
    [SerializeField] private Rigidbody sphereRb;
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float backSpeed;
    [SerializeField] private LayerMask groundLayerMask;

    [SerializeField] private GameObject particleSystem1;
    [SerializeField] private GameObject particleSystem2;
    [SerializeField] private GameObject particleSystem3;

    private float _forwardAmount;
    public float _currentSpeed;
    private float _turnAmount;
    private bool _isGrounded;

    private bool _isDrifting;
    private float _driftDirection;
    private float _driftTime;

    private float _boostTime;

    private float _redDriftCounter = 0f;
    [SerializeField] private float ScarletBurst = 0f;

    PhotonView View;

    private void Start()
    {
        sphereRb.transform.parent = null;
        View = GetComponentInParent<PhotonView>();
    }

    private void Update()
    {
        if (View.IsMine)
        {
            transform.position = sphereRb.transform.position;

            _forwardAmount = Input.GetAxis("Vertical");
            _turnAmount = Input.GetAxis("Horizontal");

            if (_forwardAmount != 0)
                Drive();
            else
                Stand();

            TurnHandler();

            if (Input.GetButtonDown("Jump") && !_isDrifting && _turnAmount != 0)
                StartDrift();

            if (_isDrifting && (Input.GetButtonUp("Jump") || (Input.GetKeyUp(KeyCode.W)) || _turnAmount == 0))
                EndDrift();

            if (_boostTime > 0)
            {
                _boostTime -= Time.deltaTime;
                if (_boostTime <= 0)
                {
                    _currentSpeed = forwardSpeed;
                }
            }
        }
    }

    private void TurnHandler()
    {
        float newRotation = _turnAmount * turnSpeed * Time.deltaTime;

        if (_currentSpeed > 0.1f)
            transform.Rotate(0, newRotation, 0, Space.World);
    }

    private void FixedUpdate()
    {
        sphereRb.AddForce(transform.forward * _currentSpeed, ForceMode.Acceleration);

        if (_isDrifting)
        {
            float driftTurnSpeed = turnSpeed * 2;
            float newRotation = _driftDirection * driftTurnSpeed * Time.deltaTime;
            transform.Rotate(0, newRotation, 0, Space.World);

            Vector3 driftForce = -transform.right * _driftDirection * _currentSpeed / 2;
            sphereRb.AddForce(driftForce, ForceMode.Acceleration);

            _driftTime += Time.deltaTime;

            UpdateParticleSystems();

            if (_driftTime > 2.7f)
            {
                _redDriftCounter += Time.deltaTime;
                if (_redDriftCounter >= 1f)
                {
                    DriftPointManager.Instance.AddPoints(Card.PointType.Red);
                    ScarletBurst += 1;
                    _redDriftCounter = 0f;
                }
            }

            if (_isDrifting && Input.GetKey(KeyCode.Space) && _turnAmount != _driftDirection)
            {
                float counterForceMagnitude = _currentSpeed * 0.3f;
                Vector3 counterForce = transform.right * -_driftDirection * counterForceMagnitude;
                sphereRb.AddForce(counterForce, ForceMode.Acceleration);
            }
        }
    }

    private void Drive()
    {
        if (_currentSpeed <= 90)
            _currentSpeed += (_forwardAmount *= forwardSpeed) / 2;
        else
            _currentSpeed -= Time.deltaTime;
        _currentSpeed = Mathf.Max(_currentSpeed, backSpeed);
    }

    private void Stand()
    {
        _currentSpeed = 0;
    }

    private void StartDrift()
    {
        _isDrifting = true;
        _driftDirection = _turnAmount;
    }

    private void EndDrift()
    {
        _isDrifting = false;

        if (_driftTime > 2.7f)
        {
            _boostTime = 2;
            _currentSpeed += ScarletBurst * 10f;
            ScarletBurst = 0;
            DriftPointManager.Instance.AddPoints(Card.PointType.Red);
        }
        else if (_driftTime > 1.7f)
        {
            _boostTime = 5;
            _currentSpeed += 10.0f;
            DriftPointManager.Instance.AddPoints(Card.PointType.Blue);
        }
        else if (_driftTime > 0.3f)
        {
            _boostTime = 1;
            _currentSpeed += 1.0f;
            DriftPointManager.Instance.AddPoints(Card.PointType.Green);
        }

        _driftTime = 0;

        // Send custom event to synchronize particle system changes
        if (View.IsMine)
        {
            View.RPC("DeactivateParticleSystems", RpcTarget.All);
        }
    }

    [PunRPC]
    private void DeactivateParticleSystems()
    {
        particleSystem1.SetActive(false);
        particleSystem2.SetActive(false);
        particleSystem3.SetActive(false);
    }

    private void UpdateParticleSystems()
    {
        if (_driftTime > 2.7f)
        {
            ActivateParticleSystem(particleSystem3);
        }
        else if (_driftTime > 1.7f)
        {
            ActivateParticleSystem(particleSystem2);
        }
        else if (_driftTime > 0.3f)
        {
            ActivateParticleSystem(particleSystem1);
        }
    }

    private void ActivateParticleSystem(GameObject activeParticleSystem)
    {
        
        activeParticleSystem.SetActive(true);

        // Call RPC to update particle systems on all clients
        if (View.IsMine)
        {
            View.RPC("UpdateParticleSystemsRPC", RpcTarget.All, activeParticleSystem.name);
        }
    }

    [PunRPC]
    private void UpdateParticleSystemsRPC(string activeParticleSystemName)
    {
        if (activeParticleSystemName == particleSystem1.name)
        {
            particleSystem1.SetActive(true);
        }
        else if (activeParticleSystemName == particleSystem2.name)
        {
            particleSystem1.SetActive(false);
            particleSystem2.SetActive(true);
        }
        else if (activeParticleSystemName == particleSystem3.name)
        {
            particleSystem2.SetActive(false);
            particleSystem3.SetActive(true);
        }
    }
}