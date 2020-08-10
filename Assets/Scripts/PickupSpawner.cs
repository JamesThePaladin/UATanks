using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    
    private Transform tf;
    private float nextSpawnTime;
    public float minX = 10f;
    public float maxX = 10f;
    public float minZ = 10f;
    public float maxZ = 10f;

    // Start is called before the first frame update
    void Start()
    {
        tf = gameObject.GetComponent<Transform>();
        GameManager.instance.pickupSpawners.Add(tf);
        nextSpawnTime = Time.time + GameManager.instance.spawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        //randomly change position
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);
        tf.position = new Vector3(randomX, 0, randomZ);

        //if current number of power ups is less than the maximum allowed
        if (GameManager.instance.currentPowerUps < GameManager.instance.maxPowerUps)
        {
            //and if enough time has passed for the next spawn time
            if (Time.time > nextSpawnTime)
            {
                int random = Random.Range(0, GameManager.instance.pickupPrefabs.Count);
                GameManager.instance.powerup = Instantiate(GameManager.instance.pickupPrefabs[random], tf.position, tf.rotation);
                nextSpawnTime = Time.time + GameManager.instance.spawnDelay;
                GameManager.instance.currentPowerUps++;
            }
        }
    }
}
