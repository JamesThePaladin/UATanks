using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPawn : Pawn
{
    //player lives
    [Header("Player Lives")]
    public int lives;

    protected override void Start()
    {
        base.Start();
        GameManager.instance.players.Add(gameObject);
    }

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
            GameManager.instance.PlayerDeath(gameObject);
        }
    }
}
