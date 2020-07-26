using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMotor : MonoBehaviour
{
    //components
    private CharacterController characterController;
    public Transform tf;
   
    // Start is called before the first frame update
    void Start()
    {
        //get components
        characterController = gameObject.GetComponent<CharacterController>();
        tf = gameObject.GetComponent<Transform>();
        
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

    
}
