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

    [SerializeField] private GameObject greenParticleSystem;
    [SerializeField] private GameObject blueParticleSystem;
    [SerializeField] private GameObject redParticleSystem;
    

    private float _forwardAmount;
    public float _currentSpeed;
    private float _turnAmount;
    private bool _isGrounded;

    private bool _isDrifting;
    private float _driftDirection;
    private float _driftTime;

    private float _redDriftCounter = 0f;
    private bool isSpeedBoostActive = false;
    private Card.PointType lastPointTypeObtained = Card.PointType.Green;

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

            if (Input.GetButtonDown("Jump") && !_isDrifting && Mathf.Abs(_turnAmount) >= 0.5)
                StartDrift();

            if (_isDrifting && (Input.GetButtonUp("Jump") || (Input.GetKeyUp(KeyCode.W)) ))
                EndDrift();

         
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isSpeedBoostActive)
            {
              ActivateSpeedBoost();
            }
        }
    }

    private void ActivateSpeedBoost()
{
    if (!isSpeedBoostActive) // Ensure a speed boost is not already active
        {
            isSpeedBoostActive = true; // Set the flag to indicate a speed boost is active
            StartCoroutine(SpeedBoostCoroutine(lastPointTypeObtained));
        }
}


 private IEnumerator SpeedBoostCoroutine(Card.PointType pointTypeToSpend)
{
    // Determine which particle system to activate based on the point type
    GameObject particleSystemToActivate = null;
    switch (pointTypeToSpend)
    {
        case Card.PointType.Green:
            particleSystemToActivate = greenParticleSystem;
            break;
        case Card.PointType.Blue:
            particleSystemToActivate = blueParticleSystem;
            break;
        case Card.PointType.Red:
            particleSystemToActivate = redParticleSystem;
            break;
    }

    // Activate the particle system locally
    if (particleSystemToActivate != null)
    {
        particleSystemToActivate.SetActive(true);
    }

    // Use RPC to activate the particle system on all clients
    if (View.IsMine)
    {
        View.RPC("ActivateParticleSystemRPC", RpcTarget.All, particleSystemToActivate.name);
    }

    while (Input.GetKey(KeyCode.LeftShift))
    {
        // Check if there are enough points to spend
        if (DriftPointManager.Instance.SpendPoints(pointTypeToSpend, 1))
        {
            // Apply a temporary speed boost
            float originalSpeed = _currentSpeed;
            _currentSpeed *= 1.5f;
            yield return new WaitForSeconds(1); // Wait for 1 second before applying the boost again
            _currentSpeed = originalSpeed;
        }
        else
        {
            // Not enough points, deactivate the particle system and stop the coroutine
            if (particleSystemToActivate != null)
            {
                particleSystemToActivate.SetActive(false);
            }
            if (View.IsMine)
            {
                View.RPC("DeactivateParticleSystemRPC", RpcTarget.All, particleSystemToActivate.name);
            }
            isSpeedBoostActive = false; // Reset the flag
            yield break;
        }
    }

    // Deactivate the particle system locally
    if (particleSystemToActivate != null)
    {
        particleSystemToActivate.SetActive(false);
    }

    // Use RPC to deactivate the particle system on all clients
    if (View.IsMine)
    {
        View.RPC("DeactivateParticleSystemRPC", RpcTarget.All, particleSystemToActivate.name);
    }

    isSpeedBoostActive = false; // Reset the flag when the Shift key is released
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
            // Increase turn speed during drift
        float driftTurnSpeed = turnSpeed * 2; // Increase turnSpeed by a factor of 2 during drift
        float newRotation = _driftDirection * driftTurnSpeed * Time.deltaTime;
        transform.Rotate(0, newRotation, 0, Space.World);

        // Apply a stronger force to the vehicle in the direction of the drift
        Vector3 driftForce = -transform.right * _driftDirection * _currentSpeed /2;
        sphereRb.AddForce(driftForce, ForceMode.Acceleration);

        _driftTime += Time.deltaTime;

        UpdateParticleSystems();

        if (_driftTime > 2.7f)
        {
            _redDriftCounter += Time.deltaTime;
            if (_redDriftCounter >= 1f)
            {
                DriftPointManager.Instance.AddPoints(Card.PointType.Red);
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
            DriftPointManager.Instance.AddPoints(Card.PointType.Red);
            lastPointTypeObtained = Card.PointType.Red;
        }
        else if (_driftTime > 1.7f)
        {
            DriftPointManager.Instance.AddPoints(Card.PointType.Blue);
            DriftPointManager.Instance.AddPoints(Card.PointType.Blue);
            lastPointTypeObtained = Card.PointType.Blue;
        }
        else if (_driftTime > 0.3f)
        {
            DriftPointManager.Instance.AddPoints(Card.PointType.Green);
            lastPointTypeObtained = Card.PointType.Green;
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


[PunRPC]
private void ActivateParticleSystemRPC(string particleSystemName)
{
    GameObject particleSystemToActivate = null;
    if (particleSystemName == greenParticleSystem.name)
    {
        particleSystemToActivate = greenParticleSystem;
    }
    else if (particleSystemName == blueParticleSystem.name)
    {
        particleSystemToActivate = blueParticleSystem;
    }
    else if (particleSystemName == redParticleSystem.name)
    {
        particleSystemToActivate = redParticleSystem;
    }

    if (particleSystemToActivate != null)
    {
        particleSystemToActivate.SetActive(true);
    }
}

[PunRPC]
private void DeactivateParticleSystemRPC(string particleSystemName)
{
    GameObject particleSystemToActivate = null;
    if (particleSystemName == greenParticleSystem.name)
    {
        particleSystemToActivate = greenParticleSystem;
    }
    else if (particleSystemName == blueParticleSystem.name)
    {
        particleSystemToActivate = blueParticleSystem;
    }
    else if (particleSystemName == redParticleSystem.name)
    {
        particleSystemToActivate = redParticleSystem;
    }

    if (particleSystemToActivate != null)
    {
        particleSystemToActivate.SetActive(false);
    }
}
}