using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakcAndForce : MonoBehaviour
{
    public Transform unit; // Reference to the unit
    public float speed = 300.0f; // Speed of the circle
    public float moveSpeed = 10.0f; // Speed of movement away from the unit
    public float maxDistance = 30.0f; // Maximum distance the sphere can move away from the player
    private Vector3 originalPosition; // Store the original position of the sphere
    private bool isMoving = false; // Indicates whether the sphere is currently moving away from the unit

    

    void Update()
    {
        originalPosition = transform.parent.position;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            // Store the original position of the sphere when Fire1 is first pressed
            if (!isMoving)
            {
                //originalPosition = transform.parent.position;
                isMoving = true;
            }

            Vector3 direction = (transform.position - unit.position).normalized;
            float distance = Vector3.Distance(transform.position, unit.position);

            // Only move the sphere if it hasn't moved too far away from the player
            if (distance <= maxDistance)
            {
                transform.position += direction * moveSpeed * Time.deltaTime;
            }

            transform.RotateAround(unit.position, Vector3.up, speed * Time.deltaTime);
        }
        else
        {
            // Smoothly return the sphere to its original position when Fire1 is released
            if (isMoving)
            {
                transform.position = Vector3.Lerp(transform.position, originalPosition, moveSpeed * Time.deltaTime);
                if ((transform.position - originalPosition).magnitude < 0.01f)
                {
                    isMoving = false;
                }
            }
        }
    }
}
