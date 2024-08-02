using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DriftPointManager : MonoBehaviour
{
    public static DriftPointManager Instance { get; private set; }

    public int GreenPoints { get; private set; }
    public int BluePoints { get; private set; }
    public int RedPoints { get; private set; }

    public TMP_Text greenPointsText;
    public TMP_Text bluePointsText;
    public TMP_Text redPointsText;

    private void Awake()
    {
        if (Instance == null)
        {
           // Instance = this;
        }
        else
        {
            Debug.LogWarning("More than one instance of DriftPointManager found!");
          //  Destroy(this.gameObject);
        }
    }

    public void UpdatePointsUI()
    {
        if (greenPointsText != null && bluePointsText != null && redPointsText!= null)
        {
        greenPointsText.text = GreenPoints.ToString();
        bluePointsText.text = BluePoints.ToString();
        redPointsText.text = RedPoints.ToString();
        }
        else{
            Debug.Log("fg");
        }
            
    }

    public void AddPoints(Card.PointType pointType)
    {
        switch (pointType)
        {
            case Card.PointType.Green:
                GreenPoints++;
                break;
            case Card.PointType.Blue:
                BluePoints++;
                break;
            case Card.PointType.Red:
                RedPoints++;
                break;
        }

        UpdatePointsUI();
    }

    public bool SpendPoints(Card.PointType pointType, int cardCost)
    {

        switch (pointType)
        {
            case Card.PointType.Green:
                if (GreenPoints >= cardCost)
                {
                    GreenPoints -= cardCost;
                    UpdatePointsUI();
                    return true;
                }
                break;
            case Card.PointType.Blue:
                if (BluePoints >= cardCost)
                {
                    BluePoints -= cardCost;
                    UpdatePointsUI();
                    return true; 

                }
                break;
            case Card.PointType.Red:
                if (RedPoints >= cardCost)
                {
                    RedPoints -= cardCost;
                    UpdatePointsUI();
                    return true;
                }
                break;
        }
            
        Debug.Log("Not enough points to spawn this unit");

        return false;
    }
}
