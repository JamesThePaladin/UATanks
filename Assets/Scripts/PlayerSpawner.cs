using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private Transform tf;

    // Start is called before the first frame update
    void Start()
    {
        //get transform
        tf = GetComponent<Transform>();
        //add it to the game manager's list
        GameManager.instance.playerSpawners.Add(tf);
    }
}
