using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Camera follow script courtesy of 
/// Stack overflow user 
/// Mukesh Saini
/// </summary>
public class CameraController : MonoBehaviour
{
    //game object camera is following
    public UnityEngine.GameObject player;
    //offset by which it follows
    private Vector3 offset;
    Vector3 playerPrevPos, playerMoveDir;
    float distance = 3f;

    // Use this for initialization
    void Start()
    {
        offset = transform.position - player.transform.position;
        distance = offset.magnitude;
        playerPrevPos = player.transform.position;
    }

    void LateUpdate()
    {
        playerMoveDir = player.transform.position - playerPrevPos;
        if (playerMoveDir != Vector3.zero)
        {
            playerMoveDir.Normalize();
            transform.position = player.transform.position - playerMoveDir * distance;

            float yy = transform.position.y; yy += 5f; 
            transform.position = new Vector3(transform.position.x, yy, transform.position.z);

            transform.LookAt(player.transform.position);

            playerPrevPos = player.transform.position;
        }
    }
}
