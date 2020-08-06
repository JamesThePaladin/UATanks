using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Animations;

public class AIController : Controller
{
    //components
    public GameObject target; //our AI's target
    public Transform targetTf; //transform for our AI's target
    protected Transform tf; //transform of our AI pawn
    public List<Transform> waypoints; //list of waypoints
    public LayerMask playerLayer;

    //data
    public int avoidanceStage = 0;
    public float avoidanceTime = 2.0f;
    private float exitTime = 0.5f; //var for CanMove exit times
    public float stateEnterTime; //variable to store the time a state was entered
    public float stateExitTime = 30.0f; //variable to hold the time for exiting states
    public float restingHealthRate; //in hp/second
    public float fleeDistance = 1.0f;
    public int currentWaypoint = 0; //for ai's current valyue in waypoint index
    public float closeEnough = 1.0f; //value forif our ai is close enough to its waypoint
    public float stopDistance; //to stop AI before they smack into our player

    public enum AIState { Chase, ChaseAndFire, CheckForFlee, Flee, LookAt, Patrol, Rest };
    public AIState aiState = AIState.Patrol;

    // Start is called before the first frame update
    void Awake()
    {
        tf = GetComponent<Transform>();
        pawn = GetComponent<Pawn>();
        motor = GetComponent<TankMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (aiState)
        {
            case AIState.Chase:
                //if avoidance stage is not zero (or if we are avoiding)
                if (avoidanceStage != 0)
                {
                    //do avoid manuevers
                    Avoidance();
                }
                else
                {
                    //otherwise chase player
                    Chase(target, Vector3.zero);
                }
                //check for transitions
                //if our health is lower than half our max health
                if (pawn.health < pawn.maxHealth * 0.5f)
                {
                    //check to see if we need to flee
                    ChangeState(AIState.CheckForFlee);
                }
                //check to see if we can still see our player
                foreach (GameObject target in GameManager.instance.players) 
                {
                    //if we can't see our player
                    if (!CanSee(target)) 
                    {
                        //go to look at state to look for them
                        ChangeState(AIState.Patrol);
                    }
                }
                break;
            case AIState.ChaseAndFire:
                //if avoidance stage is not zero (or if we are avoiding)
                if (avoidanceStage != 0)
                {
                    //do avoid manuevers
                    Avoidance();
                }
                else
                {
                    //otherwise chase the player
                    Chase(target, Vector3.zero);
                    //and shoot at them
                    pawn.Shoot(pawn.shotForce);
                }
                //check for transitions
                //if our health is lower than half our max health
                if (pawn.health < pawn.maxHealth * 0.5f)
                {
                    //check to see if we need to flee
                    ChangeState(AIState.CheckForFlee);
                }
                //check to see if we can still see our player
                foreach (GameObject target in GameManager.instance.players)
                {
                    //if we can't see our player
                    if (!CanSee(target))
                    {
                        //go to look at state to look for them
                        ChangeState(AIState.Patrol);
                    }
                }
                break;
            case AIState.Flee:
                //if avoidance stage is not zero (or if we are avoiding)
                if (avoidanceStage != 0)
                {
                    //do avoid manuevers
                    Avoidance();
                }
                else
                {
                    //flee so we don't die
                    Flee();
                }
                //check for transitions
                if (Time.time >= stateEnterTime + stateExitTime)
                {
                    ChangeState(AIState.CheckForFlee);
                }
                break;
            case AIState.LookAt:
                if (avoidanceStage != 0)
                {
                    //do avoid manuevers
                    Avoidance();
                }
                else
                {
                    motor.RotateTowards(targetTf.position, pawn.turnSpeed); 
                }
                foreach (GameObject target in GameManager.instance.players)
                {
                    if (CanSee(target))
                    {
                        SetTarget(target, target.transform);
                        ChangeState(AIState.Chase);
                    }
                }
                break;
            case AIState.Patrol:
                //if avoidance stage is not zero (or if we are avoiding)
                if (avoidanceStage != 0)
                {
                    //do avoid manuevers
                    Avoidance();
                }
                else
                {
                    //otherwise patrol
                    Patrol();
                }
                //check for transitions
                //if our health is lower than half our max health
                if (pawn.health < pawn.maxHealth * 0.5f)
                {
                    //change state to check for flee
                    ChangeState(AIState.CheckForFlee);
                }
                //else if our player is within our sense radius
                foreach (GameObject target in GameManager.instance.players)
                {
                    //if we can't see our player
                    if (CanHear(target))
                    {
                        SetTarget(target, target.transform);
                        //go to look at state to look for them
                        ChangeState(AIState.LookAt);
                    }
                }
                break;
            case AIState.CheckForFlee:
                //if avoidance stage is not zero (or if we are avoiding)
                if (avoidanceStage != 0)
                {
                    //do avoid manuevers
                    Avoidance();
                }
                else
                {
                    //otherwise check for flee conditions
                    CheckForFlee();
                }
                // Check for Transitions
                if (Time.time >= stateEnterTime + stateExitTime)
                {
                    ChangeState(AIState.Rest);
                }
                //else if our player is within our sense radius
                foreach (GameObject target in GameManager.instance.players)
                {
                    if (CanHear(target))
                    {
                        //flee so we don't die
                        ChangeState(AIState.Flee);
                    }
                }
                break;
            case AIState.Rest:
                Rest();
                if (avoidanceStage != 0) 
                {
                    Avoidance();
                }
                //check for transitions
                foreach (GameObject target in GameManager.instance.players) 
                {
                    if (CanHear(target))
                    {
                        //flee so we don't die
                        ChangeState(AIState.Flee);
                    }
                }
                //if our health is greater than or equal to our max health
                if (pawn.health >= pawn.maxHealth)
                {
                    //else if our player is within our sense radius
                    foreach (GameObject target in GameManager.instance.players)
                    {
                        //if we can't see our player
                        if (CanHear(target))
                        {
                            //go to look at state to look for them
                            ChangeState(AIState.LookAt);
                        }
                    }
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

    public bool CanHear(GameObject target)
    {
        // Create local variables and connect them to the pawn to reach the data we need from it.
        // Get data from the target pawn that says how much noise it is making
        float targetNoise = target.GetComponent<Pawn>().noise;
        float targetNoiseRange = target.GetComponent<Pawn>().noiseRange;
        float hDistance = pawn.hearingDistance;
        if (targetNoise > 0)
        {
            // If our distance is <= the distance we can hear + the distance the pawn's noise travels
            if (Vector3.Distance(target.transform.position, tf.position) <= hDistance + targetNoiseRange)
            {
                //we can hear them
                return true;
            } 
        }
         //otherwise we can't hear them, return false
        return false;
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
    public bool CanSee(GameObject player)
    {
        // Find the vector from the agent to the target
        // We do this by subtracting "destination minus origin", so that "origin plus vector equals destination."
        Vector3 agentToPlayerVector = player.transform.position - pawn.transform.position;

        // Find the angle between the direction our agent is facing (forward in local space) and the vector to the target.
        float angleToPlayer = Vector3.Angle(agentToPlayerVector, pawn.transform.right);

        // if that angle is less than our field of view
        if (angleToPlayer < pawn.viewRadius)
        {
            if (Vector3.Distance(pawn.transform.position, player.transform.position) < pawn.fieldOfView / 2)
            {
                // Raycast
                if (Physics.Raycast(pawn.transform.position, agentToPlayerVector, out RaycastHit hit, pawn.viewRadius, playerLayer))
                {
                    // If the first object we hit is our target 
                    if (hit.collider.gameObject == player)
                    {
                        // return true 
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        return false;
    }

    public void ChangeState(AIState newState)
    {
        //change state
        aiState = newState;
        //save time state was changed
        stateEnterTime = Time.time;
    }

    public void Chase(GameObject target, Vector3 offset)
    {
        //rotates toward target
        motor.RotateTowards(target.transform.position + offset, pawn.turnSpeed);
        // Check if we can move "pawn.moveSpeed" units away.
        //    We chose this distance, because that is how far we move in 1 second,
        //    This means, we are looking for collisions "one second in the future."
        float distanceToTarget = Vector3.Distance(target.transform.position + offset, tf.position);
        if ((distanceToTarget) > stopDistance)
        {
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
    }

    public void CheckForFlee()
    {
        //Do Nothing
    }

    public void Flee()
    {
        //the vector from ai to target is target position minus ai position
        Vector3 vectorToTarget = targetTf.position - tf.position;
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

    public void LastSeen(GameObject target) 
    {
        //TODO: finish this state
    }

    public void Patrol()
    {
        if (motor.RotateTowards(waypoints[currentWaypoint].position, pawn.turnSpeed))
        {
            //Do nothing!
        }
        else
        {
            //Move forward
            motor.Move(pawn.moveSpeed);
        }
        //if close to waypoint
        if (Vector3.SqrMagnitude(waypoints[currentWaypoint].position - tf.position) < closeEnough)
        {
            //and if the waypoint index hasn't been completed
            if (currentWaypoint < waypoints.Count - 1)
            {
                //move to the next patrol waypoint
                currentWaypoint++;
            }

            else
            {
                //if it has reset index
                currentWaypoint = 0;
            }
        }
    }

    public void Rest()
    {
        //increase health per second
        pawn.health += restingHealthRate * Time.deltaTime;
        //don't go over max health
        pawn.health = Mathf.Min(pawn.health, pawn.maxHealth);
    }

    private void SetTarget(GameObject newTarget, Transform newTargetTf) 
    {
        target = newTarget;
        targetTf = newTargetTf;
    }
}
