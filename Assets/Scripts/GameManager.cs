using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //variable that holds this instance of the GameManager
    public static GameManager instance;

    [Header("Lists")]
    //list to hold Pickup spawners
    public List<Transform> pickupSpawners;
    //list to hold all enemies in game
    public List<GameObject> enemies;
    //list to hold enemy spawners
    public List<GameObject> enemySpawners;
    //list for all players in the game
    public GameObject[] players;
    //list for player spawners
    public List<GameObject> playerSpawners;
    //map tile grid array
    public Room[,] grid;

    [Header("PowerUp Stuff")]
    public GameObject powerup;
    public List<GameObject> pickupPrefabs;
    public float spawnDelay = 10.0f;
    
    public float currentPowerUps;
    public float maxPowerUps = 4.0f;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) // if instance is empty
        {
            instance = this; // store THIS instance of the class in the instance variable
            DontDestroyOnLoad(this.gameObject); //keep this instance of game manager when loading new scenes
        }
        else
        {
            Destroy(this.gameObject); // delete the new game manager attempting to store itself, there can only be one.
            Debug.Log("Warning: A second game manager was detected and destrtoyed"); // display message in the console to inform of its demise
        }
    }
}
