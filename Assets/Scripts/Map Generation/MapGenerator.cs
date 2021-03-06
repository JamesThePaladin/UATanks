﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Rooms")]
    public int rows;
    public int cols;
    public float roomWidth = 50.0f;
    public float roomHeight = 50.0f;
    public GameObject[] roomPrefabs;

    [Header("Map Seed")]
    public int presetSeed = 0;
    public int mapSeed;
    public int mapOfDaySeed;

    // Start is called before the first frame update
    void Start()
    {
        //set our random map seed 
        mapSeed = (DateToInt(DateTime.Now));
        //set our map of the day seed
        mapOfDaySeed = DateToInt(DateTime.Now.Date);
        //Generate Map
        GenerateMap();
    }

    //generates the game map
    public void GenerateMap() 
    {
        //if the player wants the map of the day
        if (GameManager.instance.isMapOfTheDay)
        {
            //initialize the generator with our map of the day seed
            UnityEngine.Random.InitState(mapOfDaySeed);
        }
        else if (GameManager.instance.isRandomMap)
        {
            //Otherwise, initialize the random generator with our random seed
            UnityEngine.Random.InitState(mapSeed);
        }
        else 
        {
            //if neither of the above, use a preset
            UnityEngine.Random.InitState(presetSeed);
        }

        //clear out the grid - "which column" is our X, "which row" is our Y
        GameManager.instance.grid = new Room[cols, rows];

        //For each row...
        for (int i = 0; i < rows; i++) 
        {
            //for each column in that row...
            for (int j = 0; j < cols; j++) 
            {
                //Find the location
                float xPos = roomWidth * j;
                float zPos = roomHeight * i;
                Vector3 newPos = new Vector3(xPos, 0.0f, zPos);

                //Create a new room at appropriate location
                GameObject tempRoomObj = Instantiate(RandomRoomPrefab(), newPos, Quaternion.identity);

                //set its parent
                tempRoomObj.transform.parent = this.transform;

                //Name it something meaningful
                tempRoomObj.name = "Room_" + j + "" + i;

                //get room object
                Room tempRoom = tempRoomObj.GetComponent<Room>();

                //Open the doors
                //if we are on the bottom row, open the north door
                if (i == 0)
                {
                    tempRoom.doorNorth.SetActive(false);
                }
                else if (i == rows - 1)
                {
                    //Otherwise, if we are on the top row, open the south door
                    tempRoom.doorSouth.SetActive(false);
                }
                else 
                {
                    //Otherwise we are in the middle, so open both doors
                    tempRoom.doorNorth.SetActive(false);
                    tempRoom.doorSouth.SetActive(false);
                }
                //if we are in the first column, open the east door
                if (j == 0)
                {
                    tempRoom.doorEast.SetActive(false);
                }
                else if (j == cols - 1)
                {
                    //Otherwise, if we are on the last column row, open the west door
                    tempRoom.doorWest.SetActive(false);
                }
                else 
                {
                    //Otherwise we are in the middle, so open both doors
                    tempRoom.doorEast.SetActive(false);
                    tempRoom.doorWest.SetActive(false);
                }
                //save it to grid array
                GameManager.instance.grid[i, j] = tempRoom;
            }
        }
    }

    public int DateToInt(DateTime dateToUse) 
    {
        //add our date into an int and return it
        return dateToUse.Year + dateToUse.Month + dateToUse.Day + dateToUse.Hour + dateToUse.Minute + dateToUse.Second + dateToUse.Millisecond;
    }

    //Returns a random room
    public GameObject RandomRoomPrefab() 
    {
        return roomPrefabs[UnityEngine.Random.Range(0, roomPrefabs.Length)];
    }
}
