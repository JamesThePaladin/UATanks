﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPawn : Pawn
{
    //player lives
    public int lives;

    //player damage so we dont destroy our player's score.
    public override void TakeDamage(float damage, GameObject instigator)
    {
        //health is set equal to health decremented by damage
        health -= damage;
        //if health is equal to or below 0
        if (health <= 0)
        {
            //give the instigator points
            instigator.SendMessage("ScorePoints", points);
            //tell the game manager you died
            //TODO: GameManager function for player death
        }
    }
}
