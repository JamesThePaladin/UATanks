using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    
    private Transform tf;
    public float minX = 10f;
    public float maxX = 10f;
    public float minZ = 10f;
    public float maxZ = 10f;

    // Start is called before the first frame update
    void Start()
    {
        tf = gameObject.GetComponent<Transform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);
        tf.position = new Vector3(randomX, 0, randomZ);
    }
}
