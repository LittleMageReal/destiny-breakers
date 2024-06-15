using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public KArtController Kartcontrol;
    void Start()
    {
        Kartcontrol.isAiControlled = true;
    }

    void Update()
    {
        
        Kartcontrol._forwardAmount = 1;
        Kartcontrol._turnAmount = 1;
        Kartcontrol.Drive();
        Kartcontrol.TurnHandler();
    }
}