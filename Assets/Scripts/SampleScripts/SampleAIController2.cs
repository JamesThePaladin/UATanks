using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleAIController2 : Controller
{
    //components
    private Transform tf;
    public Transform target;

    //data
    public enum AttackMode { Chase, Flee };
    public AttackMode attackMode;
    public float fleeDistance = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        tf = pawn.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (attackMode) 
        {
            case AttackMode.Chase:
                Chase();
                break;
            case AttackMode.Flee:
                Flee();
                break;
        }
    }

    public void Chase() 
    {
        //rotates toward target
        motor.RotateTowards(target.position, pawn.turnSpeed);
        //move forward
        motor.Move(pawn.moveSpeed);
    }

    public void Flee() 
    {
        //the vector from ai to target is target position minus ai position
        Vector3 vectorToTarget = target.position - tf.position;
        //Flip this vector by -1 to get a vector AWAY from target
        Vector3 fleeToVector = -vectorToTarget;
        //normalize for a magnitude of 1
        fleeToVector.Normalize();
        //A normalized vector can be multiplied by a length to make a vector of that length
        fleeToVector *= fleeDistance;
        // We can find the position in space we want to move to by adding our vector away from our AI to our AI's position.
        //     This gives us a point that is "that vector away" from our current position.
        Vector3 fleePosition = fleeToVector + tf.position;
        motor.RotateTowards(fleePosition, pawn.turnSpeed);
        motor.Move(pawn.moveSpeed);
    }
}
