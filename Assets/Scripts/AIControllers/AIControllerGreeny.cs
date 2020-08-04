using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControllerGreeny : AIController
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
                    Chase(GameManager.instance.player, -Vector3.forward);
                }
                //check for transitions
                //if our health is lower than half our max health
                if (pawn.health < pawn.maxHealth * 0.5f)
                {
                    //check to see if we need to flee
                    ChangeState(AIState.CheckForFlee);
                }
                //else if our player is within our sense radius
                else if (Vector3.Distance(target.position, tf.position) <= aiSenseRadius)
                {
                    //chase and fire at them
                    ChangeState(AIState.ChaseAndFire);
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
                    Chase(GameManager.instance.player, -Vector3.forward);
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
                //else if our player is within our sense radius
                else if (Vector3.Distance(target.position, tf.position) > aiSenseRadius)
                {
                    //chase them
                    ChangeState(AIState.Chase);
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
                else if (Vector3.Distance(target.position, tf.position) <= aiSenseRadius)
                {
                    //chase them
                    ChangeState(AIState.Chase);
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
                //else if our player is within our sense radius
                if (Vector3.Distance(target.position, tf.position) <= aiSenseRadius)
                {
                    //flee so we don't die
                    ChangeState(AIState.Flee);
                }
                else
                {
                    //otherwise rest
                    ChangeState(AIState.Rest);
                }
                break;
            case AIState.Rest:
                Rest();
                //check for transitions
                //else if our player is within our sense radius
                if (Vector3.Distance(target.position, tf.position) <= aiSenseRadius)
                {
                    //flee so we don't die
                    ChangeState(AIState.Flee);
                }
                //otherwise if our health is greater than or equal to our max health
                else if (pawn.health >= pawn.maxHealth)
                {
                    //else if our player is within our sense radius
                    if (Vector3.Distance(target.position, tf.position) <= aiSenseRadius)
                    {
                        //chase them
                        ChangeState(AIState.Chase);
                    }
                    else
                    {
                        //if not go back to patrolling
                        ChangeState(AIState.Patrol);
                    }
                }
                break;
        }
    }
}
