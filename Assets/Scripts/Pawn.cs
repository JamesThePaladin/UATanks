using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    public GameObject shell;
    public Rigidbody shellRb;
    private Transform firingZone;

    //data
    public float shotTimerDelay;
    private float coolDownTimer;
    public float health = 100; //health for pawns
    public float moveSpeed = 3; //in meters per second
    public float turnSpeed = 180; //in degrees per second
    public float shotForce = 200; //in force applied to rb.
    public float damage = 25; //float for damage done by pawn shell's

    public void Start()
    {
        firingZone = gameObject.GetComponentInChildren<Transform>().Find("FiringZone");

        //set cooldown time
        coolDownTimer = shotTimerDelay;
    }

    // Update is called once per frame
    void Update()
    {
        coolDownTimer -= Time.deltaTime;
    }

    public void Shoot(float shotForce)
    {
        if (coolDownTimer <= 0)
        {
            //create a vector 3 for shot direction tat is equal to our firing zone's forward vector multiplied by shotForce
            Vector3 shotDir = firingZone.forward * shotForce;
            //instantiate a shell at the firing zone's position and rotation, save it in shellInstance.
            GameObject shellInstance = Instantiate(shell, firingZone.position, firingZone.rotation);
            //give the shell its damage value
            shellInstance.GetComponent<ShellScript>().damage = damage;
            //get the shellInstance's rigidbody
            shellRb = shellInstance.GetComponent<Rigidbody>();
            //add force to it equal to shot direction 
            shellRb.AddForce(shotDir);
            //destroy the instance after 3 seconds.
            Destroy(shellInstance, 3.0f);
            //reset cooldown
            coolDownTimer = shotTimerDelay;
        }
    }

    public void TakeDamage(float damage) 
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
