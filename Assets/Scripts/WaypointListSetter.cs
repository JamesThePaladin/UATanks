using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointListSetter : MonoBehaviour
{
    private Transform tf;

    void Start()
    {
        //get transform
        tf = GetComponent<Transform>();
        //add it to the GameManager's list
        GameManager.instance.waypoints.Add(tf);
    }
}
