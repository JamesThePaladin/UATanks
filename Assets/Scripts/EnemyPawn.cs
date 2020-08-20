using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPawn : MonoBehaviour
{
    private void OnDestroy()
    {
        GameManager.instance.currentEnemies--;
    }
}
