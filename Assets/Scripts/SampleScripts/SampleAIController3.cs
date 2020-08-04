using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleAIController3 : Controller
{
    public Transform target;
    private Transform tf;
    public int avoidanceStage = 0;
    public float avoidanceTime = 2.0f;
    private float exitTime = 0.5f;
    public enum AttackMode { Chase };
    public AttackMode attackMode;

    // Start is called before the first frame update
    void Start()
    {
        tf = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attackMode == AttackMode.Chase) 
        {
            if (avoidanceStage != 0)
            {
                Avoidance();
            }
            else 
            {
                Chase();
            }
        }
    }

    public void Chase() 
    {
        //rotates toward target
        motor.RotateTowards(target.position, pawn.turnSpeed);
        // Check if we can move "data.moveSpeed" units away.
        //    We chose this distance, because that is how far we move in 1 second,
        //    This means, we are looking for collisions "one second in the future."
        if (CanMove(pawn.moveSpeed))
        {
            //move forward
            motor.Move(pawn.moveSpeed);
        }
        else 
        {
            //enter obstacle avoidance stage 2
            avoidanceStage = 1;
        }
    }

    public void Avoidance() 
    {
        if (avoidanceStage == 1)
        {
            // Rotate left
            tf.Rotate(0, -pawn.turnSpeed, 0);

            // If I can now move forward, move to stage 2!
            if (CanMove(pawn.moveSpeed))
            {
                avoidanceStage = 2;
            }

            // Otherwise, we'll do this again next turn!
        }
        else if (avoidanceStage == 2)
        {
            // if we can move forward, do so
            if (CanMove(pawn.moveSpeed))
            {
                // Subtract from our timer and move
                exitTime -= Time.deltaTime;
                motor.Move(pawn.moveSpeed);

                // If we have moved long enough, return to chase mode
                if (exitTime <= 0)
                {
                    avoidanceStage = 0;
                }
            }
            else
            {
                // Otherwise, we can't move forward, so back to stage 1
                avoidanceStage = 1;
            }
        }
    }

    public bool CanMove(float speed) 
    {
        //raycast foward by the distance we sent in
        //if our raycast hits something
        if (Physics.Raycast(tf.position, tf.forward, out RaycastHit hit, speed))
        {
            //and if what we hit is not the player
            if (!hit.collider.CompareTag("Player"))
            {
                //then we can't mmove
                return false;
            }
        }
        //else return true
        return true;
    }
}
