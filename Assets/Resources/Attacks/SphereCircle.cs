using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCircle : MonoBehaviour
{
    public Transform unit; // Reference to the unit
    public float speed = 1.0f; // Speed of the circle
    public float moveSpeed = 1.0f; // Speed of movement away from the unit
    private bool isClockwise = true; // Rotation direction

    void Update()
    {
        // Change rotation direction when Fire1 is pressed
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isClockwise = !isClockwise;
        }

        // Rotate the sphere around the unit
        float rotationDirection = isClockwise ? 1 : -1;
        transform.RotateAround(unit.position, Vector3.up, speed * rotationDirection * Time.deltaTime);
    }
}
