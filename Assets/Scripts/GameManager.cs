using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //variable that holds this instance of the GameManager
    public static GameManager instance;

    [Header("Lists")]
    //list to hold all enemies in game
    public List<GameObject> enemies;
    //list for all players in the game
    public GameObject[] players;

    [Header("PowerUp Stuff")]
    public GameObject powerup;
    public Transform pickupSpawner;
    public List<GameObject> pickupPrefabs;
    public float spawnDelay = 10.0f;
    private float nextSpawnTime;
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

        pickupSpawner = GameObject.FindWithTag("PickupSpawner").GetComponent<Transform>();
    }

    void Start()
    {
        nextSpawnTime = Time.time + spawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPowerUps < maxPowerUps)
        {
            if (Time.time > nextSpawnTime)
            {
                int random = Random.Range(0, pickupPrefabs.Count);
                powerup = Instantiate(pickupPrefabs[random], pickupSpawner.position, pickupSpawner.rotation);
                nextSpawnTime = Time.time + spawnDelay;
                currentPowerUps++;
            }
        }
    }
}
