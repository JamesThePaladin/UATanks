﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMotor : MonoBehaviour
{
    //components
    private CharacterController characterController;
    public Transform tf;
    public GameObject shell;
    public Rigidbody shellRb;
    private Transform firingZone;

    // Start is called before the first frame update
    void Start()
    {
        //get components
        characterController = gameObject.GetComponent<CharacterController>();
        tf = gameObject.GetComponent<Transform>();
        firingZone = gameObject.GetComponentInChildren<Transform>().Find("FiringZone");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //function for tank movement
    public void Move(float speed) 
    {
        //create a vector 3 set equal to our direction and speed
        Vector3 speedVector = tf.forward * speed;
        //call simple move and give it our vector
        characterController.SimpleMove(speedVector);

    }

    //tank turning functions
    public void Turn(float speed) 
    {
        //create a vector 3 set equal to y1 multiplied by speed and adjust to seconds
        Vector3 rotateVector = Vector3.up * speed * Time.deltaTime;
        //rotate our tank in local space by this value
        tf.Rotate(rotateVector, Space.Self);
    }

    public void Shoot(float shotForce)
    {
        //vector for shot direction set equal to firingZone's foward axis * shotForce
        Vector3 shotDir = firingZone.forward * shotForce;
        //instantiate a tank shell at the firing zone's position and rotation
        GameObject shellInstance = Instantiate(shell, firingZone.position, firingZone.rotation);
        //get the rigidbody of the newly instantiated tank shell
        shellRb = shellInstance.GetComponent<Rigidbody>();
        //add shot direction to it has a force
        shellRb.AddForce(shotDir);
        //destroy it after 3 seconds
        Destroy(shellInstance, 3.0f);
    }
}
