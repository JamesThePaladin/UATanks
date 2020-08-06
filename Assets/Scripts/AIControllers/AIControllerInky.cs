using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControllerInky : AIController
{
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
                    if (!CanSee(target.GetComponent<GameObject>()))
                    {
                        //go to look at state to look for them
                        ChangeState(AIState.LookAt);
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
                    if (!CanSee(target.GetComponent<GameObject>()))
                    {
                        //go to look at state to look for them
                        ChangeState(AIState.LookAt);
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
                    if (CanHear(target.GetComponent<GameObject>()))
                    {
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
                    //else if our player is within our sense radius
                    foreach (GameObject target in GameManager.instance.players)
                    {
                        //if we can't see our player
                        if (CanHear(target.GetComponent<GameObject>()))
                        {
                            //go to look at state to look for them
                            ChangeState(AIState.LookAt);
                        }
                    }
                }
                break;
        }
    }
}
