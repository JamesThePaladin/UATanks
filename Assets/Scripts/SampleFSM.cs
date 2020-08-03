using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleFSM : Controller
{
    public Transform target;
    private Transform tf;
    public int avoidanceStage = 0;
    public float avoidanceTime = 2.0f;
    private float exitTime = 0.5f; //var for CanMove exit times
    public float stateEnterTime; //variable to store the time a state was entered
    public float stateExitTime = 30.0f; //variable to hold the time for exiting states
    public float aiSenseRadius;
    public float restingHealthRate; //in hp/second
    public float fleeDistance = 1.0f;

    public enum AIState { Chase, ChaseAndFire, CheckForFlee, Flee, Rest };
    public AIState aiState = AIState.Chase;

    // Start is called before the first frame update
    void Start()
    {
        tf = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (aiState) 
        {
            case AIState.Chase:
                if (avoidanceStage != 0)
                {
                    Avoidance();
                }
                else
                {
                    Chase();
                }
                //check for transitions
                if (pawn.health < pawn.maxHealth * 0.5f)
                {
                    ChangeState(AIState.CheckForFlee);
                }
                else if (Vector3.Distance(target.position, tf.position) <= aiSenseRadius)
                {
                    ChangeState(AIState.ChaseAndFire);
                }
                break;
            case AIState.ChaseAndFire:
                if (avoidanceStage != 0)
                {
                    Avoidance();
                }
                else
                {
                    Chase();
                    pawn.Shoot(pawn.shotForce);
                }
                //check for transitions
                if (pawn.health < pawn.maxHealth * 0.5f)
                {
                    ChangeState(AIState.CheckForFlee);
                }
                else if (Vector3.Distance(target.position, tf.position) > aiSenseRadius)
                {
                    ChangeState(AIState.Chase);
                }
                break;
            case AIState.Flee:
                if (avoidanceStage != 0)
                {
                    Avoidance();
                }
                else
                {
                    Flee(); 
                }
                //check for transitions
                if (Time.time >= stateEnterTime + stateExitTime) 
                {
                    ChangeState(AIState.CheckForFlee);
                }
                break;
            case AIState.CheckForFlee:
                if (avoidanceStage != 0)
                {
                    Avoidance();
                }
                else
                {
                    CheckForFlee(); 
                }
                // Check for Transitions
                if (Vector3.Distance(target.position, tf.position) <= aiSenseRadius)
                {
                    ChangeState(AIState.Flee);
                }
                else
                {
                    ChangeState(AIState.Rest);
                }
                break;
            case AIState.Rest:
                Rest();
                //check for transitions
                if (Vector3.Distance(target.position, tf.position) <= aiSenseRadius)
                {
                    ChangeState(AIState.Flee);
                }
                else if (pawn.health >= pawn.maxHealth) 
                {
                    ChangeState(AIState.Chase);
                }
                break;
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

    public void ChangeState(AIState newState) 
    {
        //change state
        aiState = newState;
        //save time state was changed
        stateEnterTime = Time.time;
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

    public void CheckForFlee() 
    {
        //TODO: write CheckForFlee state
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

    public void Rest() 
    {
        //increase health per second
        pawn.health += restingHealthRate * Time.deltaTime;
        //don't go over max health
        pawn.health = Mathf.Min(pawn.health, pawn.maxHealth);
    }
}
