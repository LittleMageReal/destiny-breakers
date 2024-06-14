
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiSkellet : MonoBehaviour
{
    
    public float attackRange = 5f;
    private int currentWaypoint = 0;
    private NavMeshAgent agent;
    public Transform player;

    public List<Transform> waypoints;

    void Start()
    {

        GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag("Waypoint");
        waypoints = new List<Transform>();

        foreach (GameObject waypointObject in waypointObjects)
        {
            waypoints.Add(waypointObject.transform);
        }



        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        GoToNextWaypoint();


    }

    void GoToNextWaypoint()
    {
        if (waypoints.Count == 0)
            return;
        agent.destination = waypoints[currentWaypoint].position;
        currentWaypoint = (currentWaypoint + 1) % waypoints.Count;
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GoToNextWaypoint();
        if (Vector3.Distance(player.position, transform.position) <= attackRange)
        {
            // Attack player
            Debug.Log("Attack player!");
        }
    }
}
