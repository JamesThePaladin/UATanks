using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public List<GameObject> pickupPrefabs;
    public float spawnDelay;
    private float nextSpawnTime;
    private Transform tf;

    // Start is called before the first frame update
    void Start()
    {
        tf = gameObject.GetComponent<Transform>();
        nextSpawnTime = Time.time + spawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawnTime) 
        {
            int random = Random.Range(0, pickupPrefabs.Count);
            GameObject powerup = Instantiate(pickupPrefabs[random], tf.position, tf.rotation);
            nextSpawnTime = Time.time + spawnDelay;
        }
    }
}
