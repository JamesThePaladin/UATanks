using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
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
    public List<GameObject> players;
    //list for player prefabs
    public List<GameObject> playerPrefabs;
    //list for player spawners
    public List<Transform> playerSpawners;
    //list for player high scores
    public List<ScoreData> highScores;
    //bool for if the game is multiplayer
    public bool isMultiplayer;
    
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

    //TODO figure out how to instantiate players at the start.

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

    void Start()
    {
        GameStart();
    }

    /// <summary>
    /// handles game over once all players have 0 lives
    /// </summary>
    public void GameOver() 
    {
        //TODO: make game over scene
        foreach (GameObject _player in players)
        {
            ScoreData _score = new ScoreData();
            _score.playerName = "player_" + players[0];
            _score.score = _player.GetComponent<PlayerPawn>().score;
            highScores.Add(_score);
        }

        SaveHighScores();
    }

    /// <summary>
    /// handles multiplayer at the start of the game
    /// </summary>
    public void GameStart() 
    {
        if (isMultiplayer)
        {
            //spawn player 1
            GameObject player1 = Instantiate(playerPrefabs[0], playerSpawners[0].position, playerSpawners[0].rotation);
            //get player 1's camera
            Camera player1Camera = player1.GetComponentInChildren<Camera>();
            //set create variables for rect values
            float Ydelta = 0.5f;
            float Hdelta = 0.5f;
            //set rect values
            player1Camera.rect = new Rect(0, Ydelta, 1, Hdelta);

            //spawn player 2
            GameObject player2 = Instantiate(playerPrefabs[1], playerSpawners[1].position, playerSpawners[1].rotation);
        }
        else 
        {
            GameObject player1 = Instantiate(playerPrefabs[0], playerSpawners[0].position, playerSpawners[0].rotation);
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
            GameOver();
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

    public void SaveHighScores() 
    {
        highScores.Sort();
        highScores.Reverse();
        highScores.GetRange(0, 3);

        foreach (ScoreData _score in highScores) 
        {
            int highScore = _score.score;
            PlayerPrefs.SetInt("High Score", highScore);
        }
    }
}
