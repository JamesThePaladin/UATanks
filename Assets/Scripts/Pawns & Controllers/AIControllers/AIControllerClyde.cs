using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControllerClyde : AIController
{
    // Update is called once per frame
    void Update()
    {
        switch (aiState)
        {
            case AIState.Chase:
                //check to see if we can still see our player
                if (!CanSee(target))
                {
                    ChangeState(AIState.Patrol);
                    return;
                }
                //if avoidance stage is not zero (or if we are avoiding)
                if (avoidanceStage != 0)
                {
                    //do avoid manuevers
                    Avoidance();
                }
                else
                {
                    //otherwise chase player
                    if (Chase(target, Vector3.forward))
                    {
                        //within range
                        ChangeState(AIState.ChaseAndFire);
                        return;
                    }
                }
                //check for transitions
                //if our health is lower than half our max health
                if (pawn.health < pawn.maxHealth * 0.5f)
                {
                    //check to see if we need to flee
                    ChangeState(AIState.CheckForFlee);
                    break;
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
                    //and shoot at them
                    pawn.Shoot(pawn.shotForce);
                    //otherwise chase the player
                    Chase(target, Vector3.forward);
                }
                //check for transitions
                //if our health is lower than half our max health
                if (pawn.health < pawn.maxHealth * 0.5f)
                {
                    //check to see if we need to flee
                    ChangeState(AIState.CheckForFlee);
                    break;
                }

                //if we can't see our player
                if (!CanSee(target))
                {
                    //go to look at state to look for them
                    ChangeState(AIState.Patrol);
                    return;
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
                    break;
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

                if (CanSee(target))
                {
                    SetTarget(target, target.transform);
                    ChangeState(AIState.Chase);
                    return;
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
                    break;
                }
                //else if our player is within our sense radius
                foreach (GameObject _player in GameManager.instance.players)
                {
                    //if we can't see our player
                    if (CanHear(_player))
                    {
                        SetTarget(_player, _player.transform);
                        //go to look at state to look for them
                        ChangeState(AIState.LookAt);
                        return;
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
                    break;
                }
                //else if our player is within our sense radius
                foreach (GameObject target in GameManager.instance.players)
                {
                    if (CanHear(target))
                    {
                        //flee so we don't die
                        ChangeState(AIState.Flee);
                        return;
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
                foreach (GameObject _player in GameManager.instance.players)
                {
                    if (CanHear(_player))
                    {
                        //flee so we don't die
                        ChangeState(AIState.Flee);
                        return;
                    }
                }
                //if our health is greater than or equal to our max health
                if (pawn.health >= pawn.maxHealth)
                {
                    //else if our player is within our sense radius
                    foreach (GameObject _player in GameManager.instance.players)
                    {
                        //if we can't see our player
                        if (CanHear(_player))
                        {
                            //go to look at state to look for them
                            ChangeState(AIState.LookAt);
                            return;
                        }
                    }
                }
                break;
        }
    }
}