using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private Transform tf;

    // Start is called before the first frame update
    void Awake()
    {
        //get transform
        tf = GetComponent<Transform>();
        //add it to the game manager's list
        GameManager.instance.playerSpawners.Add(tf);
    }

    private void OnDestroy()
    {
        GameManager.instance.playerSpawners.Remove(tf);
    }
}
