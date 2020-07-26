using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //variable that holds this instance of the GameManager
    public static GameManager instance;
    //variable for player
    public GameObject player;
    //reference to score text
    //public Text scoreText;
    //list to hold all enemies in game
    public List<GameObject> enemies;

    // Start is called before the first frame update
    void Start()
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

        if (player == null) //if player slot is empty
        {
            player = GameObject.FindWithTag("Player"); //fill it with player
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) //if player slot is empty
        {
            player = GameObject.FindWithTag("Player"); //fill it with player
        }
    }

    //private void FixedUpdate()
    //{
    //    //to find the text fields when the UI is destroyed on a continue.
    //    if (scoreText == null)
    //    {
    //        scoreText = GameObject.FindWithTag("ScoreText").GetComponent<Text>();
    //    }
    //}
}
