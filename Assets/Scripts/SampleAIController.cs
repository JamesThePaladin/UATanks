using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleAIController : Controller
{
    //components
    private Transform tf;
    public List<Transform> waypoints;

    //data
    private int currentWaypoint = 0; //for ai's current valyue in waypoint index
    public float closeEnough = 1.0f; //value forif our ai is close enough to its waypoint

    void Awake()
    {
        tf = GetComponent<Transform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (motor.RotateTowards(waypoints[currentWaypoint].position, pawn.turnSpeed))
        {
            //Do nothing!
        }
        else 
        {
            //Move forward
            motor.Move(pawn.moveSpeed);
        }
        //if close to waypoint
        if (Vector3.SqrMagnitude (waypoints[currentWaypoint].position - tf.position) < closeEnough) 
        {
            //and if the waypoint index hasn't been completed
            if (currentWaypoint < waypoints.Count - 1)
            {
                //move to the next patrol waypoint
                currentWaypoint++;
            }

            else
            {
                //if it has reset index
                currentWaypoint = 0;
            }
        }
    }
}
