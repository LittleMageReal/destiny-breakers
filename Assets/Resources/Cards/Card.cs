using UnityEngine;
[CreateAssetMenu(fileName = "NewCard", menuName = "Unit")]
public class Card : ScriptableObject
{
    public string cardName;
    public Sprite cardImage;
    public int cardCost;
    public GameObject unitPrefab;
    public enum PointType { Green, Blue, Red }
    public PointType pointType;

    public enum spawnPosition { Follow, Stand, Artifact, Effect}
    public spawnPosition spawnType;
    public bool Move;
    public bool Token;
    public bool canBeReturned = false;
    public bool isActive; 
    public string cardEffect;
}

