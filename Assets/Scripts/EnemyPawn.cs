using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPawn : MonoBehaviour
{
    private void OnDestroy()
    {
        //TODO: add death sounds
        GameManager.instance.currentEnemies--;
    }
}
