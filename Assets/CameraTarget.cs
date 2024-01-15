using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraTarget : MonoBehaviour
{
    public Camera cam;
    public float threshold = 5f;

    public Transform player;

    private void Update()
    {
        if (player == null)
        {
            // Find the player object that belongs to the local player
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject playerObject in playerObjects)
            {
                player = playerObject.transform;
                break;
            }
        }

        if (player != null)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 targetPos = (player.position + hit.point) / 2f;

                targetPos.x = Mathf.Clamp(targetPos.x, -threshold + player.position.x, threshold + player.position.x);
                targetPos.z = Mathf.Clamp(targetPos.z, -threshold + player.position.z, threshold + player.position.z);

                transform.position = targetPos;
            }
        }

    }
}
