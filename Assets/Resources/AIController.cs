using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
[SerializeField] private List<Transform> targetPositionsList; // List of target positions
private int currentTargetIndex = 0; // Current target index
public float arrivalThreshold = 1f; // Distance threshold for considering arrival
public KArtController kartController; // Reference to the KArtController script
public Vector3 targetPosition; // Target player's transform

void Update()
{
SetTargetPosition(targetPositionsList[currentTargetIndex].position);

float forwardAmount = 0f;
float turnAmount = 0f;

float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
Vector3 DirectionToMove = (targetPosition - transform.position).normalized;
float dot = Vector3.Dot(transform.forward, DirectionToMove);
if (dot > 0){
forwardAmount = 1f;
}
else {
   //if target to far behind turn around via going in front
   float reverseDistance = 25f;
   if (distanceToTarget > reverseDistance){
      forwardAmount = 1f;
   }
   else {
      forwardAmount = -1f;
   }
}
float AngleToTarget = Vector3.SignedAngle(transform.forward, DirectionToMove, Vector3.up);
if (AngleToTarget > 0){
turnAmount = 1f;
}
else {
turnAmount = -1f;
}

float driftangle = Mathf.Abs(AngleToTarget);
if (driftangle > 40)
{
   kartController.StartDrift();
}
else {
   kartController.EndDrift();
}

if (driftangle < 5){
   kartController.shiftpresed = true;
   kartController.ActivateSpeedBoost();
}
else {
   kartController.shiftpresed = false;
}

kartController._forwardAmount = forwardAmount;
kartController._turnAmount = turnAmount;
kartController.Drive();
kartController.TurnHandler();

}
public void SetTargetPosition(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
        ChangeTarget();
        if (currentTargetIndex >= targetPositionsList.Count) // Loop back to the first target
        {
            currentTargetIndex = 0;
        }
    }

public void ChangeTarget(){
   float distanceToCurrentTarget = Vector3.Distance(transform.position, targetPosition);
    if (distanceToCurrentTarget <= arrivalThreshold)
    {
     currentTargetIndex++; // Move to the next target
    }
}
}