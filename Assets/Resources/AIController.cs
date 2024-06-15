using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
   public KArtController kartController; // Reference to the KArtController script

    void Update()
    {
       kartController._forwardAmount = 1.0f; 
       kartController._turnAmount = 1.0f;
       kartController.Drive();
       kartController.TurnHandler();
    }
}