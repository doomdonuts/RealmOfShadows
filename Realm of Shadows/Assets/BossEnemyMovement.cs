using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyMovement : MonoBehaviour
{

    private EnemyBehavior enemyBehavior;
    private EnemyBehavior.EnemyMovementDelegate movementMethod;

    void Start()
    {

        enemyBehavior = gameObject.GetComponent<EnemyBehavior>();
        movementMethod = bossEnemyMovement;

    }

    void Update()
    {
        enemyBehavior.ExecuteEnemyMovement(movementMethod);
    }

    // Start is called before the first frame update
    public void bossEnemyMovement()
    {

        // The boss doesn't move so this does nothing

    }

}
