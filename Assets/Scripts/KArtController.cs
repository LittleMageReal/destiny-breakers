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
            // GroundCheckAndNormalHandler();


            if (Input.GetButtonDown("Jump") && !_isDrifting && _turnAmount != 0)
                StartDrift();

            if (_isDrifting && (Input.GetButtonUp("Jump") || (Input.GetKeyUp(KeyCode.W)) || _turnAmount == 0)) //|| _turnAmount == 0)
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

    //private void GroundCheckAndNormalHandler()
    // {
    // RaycastHit hit;
    // _isGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1, groundLayerMask);
    // transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation, 0.1f);
    // }
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
            float driftTurnSpeed = turnSpeed * 2; // Increase turn speed while drifting.
            float newRotation = _driftDirection * driftTurnSpeed * Time.deltaTime;
            transform.Rotate(0, newRotation, 0, Space.World);

            Vector3 driftForce = -transform.right * _driftDirection * _currentSpeed / 2;
            sphereRb.AddForce(driftForce, ForceMode.Acceleration);

            _driftTime += Time.deltaTime;


            if (_driftTime > 2.7f)
            {
                particleSystem1.SetActive(false);
                particleSystem2.SetActive(false);
                particleSystem3.SetActive(true);
            }
            else if (_driftTime > 1.7f)
            {
                particleSystem1.SetActive(false);
                particleSystem2.SetActive(true);
                particleSystem3.SetActive(false);
            }
            else if (_driftTime > 0.3f)
            {
                particleSystem1.SetActive(true);
                particleSystem2.SetActive(false);
                particleSystem3.SetActive(false);
            }


            if (_driftTime > 2.7f) // If in red drift
            {
                _redDriftCounter += Time.deltaTime; // Increment counter by the time passed since the last frame
                if (_redDriftCounter >= 1f) // If a second has passed
                {
                    DriftPointManager.Instance.AddPoints(Card.PointType.Red); // Add a red point
                    ScarletBurst += 1;
                    _redDriftCounter = 0f; // Reset the counter
                }
            }


            if (_isDrifting && Input.GetKey(KeyCode.Space) && _turnAmount != _driftDirection)
            {
                float counterForceMagnitude = _currentSpeed * 0.3f; // Adjust this value as needed
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

        if (_driftTime > 2.7f) // Drifted for more than 5 seconds
        {
            _boostTime = 2;
            _currentSpeed += ScarletBurst * 10f; // Large speed boost
            ScarletBurst = 0;
            DriftPointManager.Instance.AddPoints(Card.PointType.Red);
        }
        else if (_driftTime > 1.7f) // Drifted for more than 2 seconds
        {
            _boostTime = 5;
            _currentSpeed += 10.0f; // Medium speed boost
            DriftPointManager.Instance.AddPoints(Card.PointType.Blue);
        }
        else if (_driftTime > 0.3f)
        {
            _boostTime = 1;
            _currentSpeed += 1.0f; // Small speed boost
            DriftPointManager.Instance.AddPoints(Card.PointType.Green);
        }

        _driftTime = 0;

        particleSystem1.SetActive(false);
        particleSystem2.SetActive(false);
        particleSystem3.SetActive(false);
    }
}
