using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    //components
    [Header("Tank Shell")]
    public GameObject shell; //game object for shell instantiation
    public Rigidbody shellRb; //rigidbody of instantiated shell
    private Transform firingZone; //firing zone empty attached to pawn

    //data
    [Header("Shot Timers")]
    public float shotTimerDelay; //timer delay on fire rate cooldown
    public float shellTimeOut; //time before shelld estroys itself aautomatically
    private float coolDownTimer; //cool down timer for firing

    [Header("Tank Stats")]
    public float health = 100; //health for pawns
    public float moveSpeed = 3; //in meters per second
    public float turnSpeed = 180; //in degrees per second
    public float shotForce = 200; //in force applied to rb.
    public float damage = 25; //float for damage done by pawn shell's
    public int points; //how many points this pawn is worth when destroyed
    public int score; //the personal score of this pawn

    protected virtual void Start()
    {

        //get the transform of pawn's firing zone.
        firingZone = gameObject.GetComponentInChildren<Transform>().Find("FiringZone");
        //set cooldown time
        coolDownTimer = shotTimerDelay;
    }

    // Update is called once per frame
    void Update()
    {
        //cool down timer decrememnted by seconds past
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
            //assign instigator to shell
            shellInstance.GetComponent<ShellScript>().instigator = gameObject;
            //get the shellInstance's rigidbody
            shellRb = shellInstance.GetComponent<Rigidbody>();
            //add force to it equal to shot direction 
            shellRb.AddForce(shotDir);
            //destroy the instance after designer set amount of time
            Destroy(shellInstance, shellTimeOut);
            //reset cooldown
            coolDownTimer = shotTimerDelay;
        }
    }

    //function for pawn's taking damage
    public void TakeDamage(float damage, GameObject instigator) 
    {
        //health is set equal to health decremented by damage
        health -= damage;
        //if health is equal to or below 0
        if (health <= 0)
        {
            //give the instigator points
            instigator.SendMessage("ScorePoints", points);
            //destroy game object
            Destroy(gameObject);
        }
    }

    //takes in points from other objects and adds it to the player's score
    public void ScorePoints(int addPoints)
    {
        //add points to player score
        score += addPoints;
    }
}
