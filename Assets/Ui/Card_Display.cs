using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class Card_Display : MonoBehaviour
{
    //public Card card;
    public TMP_Text Cost;
    public Image Art;

    public Image Panel;
    public GameObject Chain;

    public void ActiveChain(Card card)
    {
      Chain.SetActive(card.isActive); 
    }

    public void SetPanelColor(Card.PointType pointType)
    {
        switch (pointType)
        {
            case Card.PointType.Green:
                Panel.color = Color.green;
                break;
            case Card.PointType.Blue:
                Panel.color = Color.cyan;
                break;
            case Card.PointType.Red:
                Panel.color = Color.magenta;
                break;
        }
    }
}
