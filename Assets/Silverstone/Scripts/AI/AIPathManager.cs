using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathManager : MonoBehaviour
{
    [SerializeField] GameObject[] Waypoints;
    
    int CurrentWaypointIndex = 0;
    
    public float NormalDistanceToGround = 0.85f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public Vector3 GetNextWaypointPosition()
    {
        if (CurrentWaypointIndex == Waypoints.Length - 1)
        {
            CurrentWaypointIndex = -1;
        }
        CurrentWaypointIndex++;
        Vector3 Destination = FindGroundPosition(Waypoints[CurrentWaypointIndex].transform.position);
        Destination.y += NormalDistanceToGround;
        return Destination;
    }
    
    public Vector3 GetCurrentWaypointPosition()
    {
        Vector3 Destination = FindGroundPosition(Waypoints[CurrentWaypointIndex].transform.position);
        Destination.y += NormalDistanceToGround;
        return Destination;
    }
    
    Vector3 FindGroundPosition(Vector3 Position)
    {
        RaycastHit Hit;
        Physics.Raycast(Position, Vector3.down, out Hit, 100f, LayerMask.GetMask("Ground"));
        return Hit.point;
    }
}
