using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private Transform tf;
    private float nextSpawnTime;

    // Start is called before the first frame update
    void Awake()
    {
        tf = GetComponent<Transform>();
        GameManager.instance.enemySpawners.Add(gameObject);
        nextSpawnTime = Time.time + GameManager.instance.enemySpawnDelay;
    }

    void Update()
    {
        //if the number of current enemies is less than the number of max enemies
        if (GameManager.instance.currentEnemies < GameManager.instance.maxEnemies) 
        {
            //and if the time is greater than or equal to the next set spawn time
            if (Time.time >= nextSpawnTime)
            {
                //declare a random int between 0 and the max number of enemy prefabs in their list
                int random = Random.Range(0, GameManager.instance.enemyPrefabs.Count);
                //instantiate an enemy using our random int at this objects position
                GameObject enemy = Instantiate(GameManager.instance.enemyPrefabs[random], tf.position, tf.rotation);
                //name it something meaningful, in this case, the name of the prefab it chose
                //and the number it is in the current enemies count
                enemy.name = GameManager.instance.enemyPrefabs[random] + "_" + GameManager.instance.currentEnemies;
                //add it to the GameManager's list of enemies
                GameManager.instance.enemies.Add(enemy);
                //set the next spawn time equal to now plus enemy spawn delay
                nextSpawnTime = Time.time + GameManager.instance.enemySpawnDelay;
                //increment the number of current enemies
                GameManager.instance.currentEnemies++;
            }
        }
    }

    private void OnDestroy()
    {
        GameManager.instance.enemySpawners.Remove(gameObject);
    }
}
