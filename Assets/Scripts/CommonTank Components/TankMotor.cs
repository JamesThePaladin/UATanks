using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;

public class TankMotor : MonoBehaviour
{
    //components
    private CharacterController characterController;
    public Transform tf;
    public Pawn pawn;

    // Start is called before the first frame update
    void Start()
    {
        //get components
        characterController = GetComponent<CharacterController>();
        tf = GetComponent<Transform>();
        pawn = GetComponent<Pawn>();
        
    }

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
        pawn.noise++;
    }

    //tank turning functions
    public void Turn(float speed) 
    {
        //create a vector 3 set equal to y1 multiplied by speed and adjust to seconds
        Vector3 rotateVector = Vector3.up * speed * Time.deltaTime;
        //rotate our tank in local space by this value
        tf.Rotate(rotateVector, Space.Self);
    }

    //RotateTowards (Target, Speed) - rotate towards the target (if possible).
    //If we rotate, then returns true. If we can't rotate returns false.
    public bool RotateTowards(Vector3 target, float speed) 
    {
        //find the vector to our target by finding the difference between the target position and our position
        Vector3 vectorToTarget = target - tf.position;
        //find the quaternion that looks down that vector
        Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget);
        if (targetRotation == tf.rotation)
        {
            return false;
        }
        else 
        {
            tf.rotation = Quaternion.RotateTowards(tf.rotation, targetRotation, pawn.turnSpeed * Time.deltaTime);
            return true;
        }
    }
}
