using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointListSetter : MonoBehaviour
{
    private Transform tf;

    void Start()
    {
        tf = GetComponent<Transform>();
        GameManager.instance.waypoints.Add(tf);
    }
}
