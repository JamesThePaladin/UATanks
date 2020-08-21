using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPawn : Pawn
{
    public AudioSource deathSound;

    private void OnDestroy()
    {
        deathSound.Play();
        GameManager.instance.currentEnemies--;
        GameManager.instance.enemies.Remove(gameObject);
    }
}
