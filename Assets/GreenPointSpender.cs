using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class GreenPointSpender : MonoBehaviourPunCallbacks
{
    public DriftPointManager driftPointManager;
    public WillScript willScript;

    private void Start()
    {
        // Initialize references to DriftPointManager and WillScript
        // These should be assigned via the Unity Editor or another initialization method
        if (driftPointManager == null)
        {
            Debug.LogError("DriftPointManager reference not assigned.");
        }
        if (willScript == null)
        {
            Debug.LogError("WillScript reference not assigned.");
        }
        SpendAllGreenPointsAndSetWill();
    }

    public void SpendAllGreenPointsAndSetWill()
    {
            // Calculate the new Will value based on the number of points spent
            int pointsSpent = driftPointManager.GreenPoints;
            int newWillValue = pointsSpent * 100;

            // Spend all green points
            driftPointManager.SpendPoints(Card.PointType.Green, pointsSpent);

            // Set the unit's Will value
            willScript.Will = newWillValue;
    }
}
