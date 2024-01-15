using UnityEngine;
using Photon.Pun;

public class Aiming : MonoBehaviourPunCallbacks
{
    private new PhotonView photonView;
    

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            // Cast a ray from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray intersects with any collider
            if (Physics.Raycast(ray, out hit))
            {
                // Get the direction from the object's position to the hit point
                Vector3 directionToMouse = hit.point - transform.position;

                // Get the rotation that rotates from the current direction of the object to the direction of the hit point
                Quaternion rotation = Quaternion.LookRotation(directionToMouse, Vector3.up);

                // Apply the rotation to the object
                transform.rotation = rotation;
            }
        }

        else
        {
            Debug.Log("ViewNotFound");
        }
    }
    
}


// снизу тот-же самый код который тоже позволяет целиться, только нижнй еще считает дистанцию от рейкаста до объекта и когда выходит за установленный предел выравниевает юнита по одной оси (позволяет стрелять ровно прямо)

// public float maxDistance = 10f; // The maximum distance at which the rotation should be set to 0

// void Update()
//  {
// Cast a ray from the mouse position
//    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//   RaycastHit hit;

// Check if the ray intersects with any collider
//   if (Physics.Raycast(ray, out hit))
//   {
// Get the direction from the object's position to the hit point
//       Vector3 directionToMouse = hit.point - transform.position;

// Get the rotation that rotates from the current direction of the object to the direction of the hit point
//    Quaternion rotation = Quaternion.LookRotation(directionToMouse, Vector3.up);

// Check if the distance between the object and the hit point is greater than the maximum distance
//   if (Vector3.Distance(transform.position, hit.point) > maxDistance)
//   {
// Set the y rotation to 0
//      rotation.eulerAngles = new Vector3(0f, rotation.eulerAngles.y, rotation.eulerAngles.z);
//   }

// Apply the rotation to the object
//    transform.rotation = rotation;
// }
// }

