using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //variable that holds this instance of the GameManager
    public static GameManager instance;

    [Header("Map Stuff")]
    //map tile grid array
    public Room[,] grid;
    //bool for map of the day seed
    public bool isMapOfTheDay = false;
    //bool for random map seed
    public bool isRandomMap = true;

    [Header("Enemy Stuff")]
    //list to hold all enemies in game
    public List<GameObject> enemies;
    //list of all enemy prefabs to spawn as enemies
    public List<GameObject> enemyPrefabs;
    //list to hold enemy spawners
    public List<GameObject> enemySpawners;
    //list of waypoints for patrolling
    public List<Transform> waypoints;
    //int for currrent enemies on the map
    public int currentEnemies;
    //int for max enemies on the map
    public int maxEnemies;
    //spawn cooldown for enemy spawns
    public float enemySpawnDelay;

    [Header("Player Stuff")]
    //list for all players in the game
    public GameObject[] players;
    //list for player spawners
    public List<Transform> playerSpawners;
    
    [Header("PowerUp Stuff")]
    //list to hold Pickup spawners
    public List<Transform> pickupSpawners;
    public GameObject powerup;
    public List<GameObject> pickupPrefabs;
    public float spawnDelay = 10.0f;
    public float currentPowerUps;
    public float maxPowerUps = 4.0f;

    [Header("Sound Stuff")]
    private int sfxVolume;
    private int musicVolume; 

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

    /// <summary>
    /// Decrements the number of lives a player has. if lives equal zero causes game over.
    /// </summary>
    /// <param name="_player"></param>
    public void LoseLife(GameObject _player)
    {
        //get player lives
        int lives = _player.GetComponent<PlayerPawn>().lives;
        //minus a life
        lives--;

        //if lives are less than or equal to 0 game over
        if (lives <= 0)
        {
            Application.Quit();
        }
    }

    /// <summary>
    /// calls LoseLife();, then sets health = max health, then moves player to random player spawner.
    /// </summary>
    /// <param name="_player"></param>
    public void PlayerDeath(GameObject _player)
    {
        //lose a life
        LoseLife(_player);
        //reset health
        ResetHealth(_player);
        //pick a random spawn point from the list
        int random = Random.Range(0, playerSpawners.Count -1);
        //send the player to their last checkpoint
        _player.transform.position = playerSpawners[random].transform.position;
    }

    /// <summary>
    /// sets player health to max after death
    /// </summary>
    /// <param name="_player"></param>
    public void ResetHealth(GameObject _player) 
    {
        //get player health and max health
        float health = _player.GetComponent<PlayerPawn>().health;
        float maxHealth = _player.GetComponent<PlayerPawn>().maxHealth;
        //set health equal to max health
        health = maxHealth;
    }
}
