using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : EnemyBaseState
{
    float timeleft = 3f;
    public override void EnterState(PatrolingEnemy enemyAI)
    {
        Debug.Log("STATE CHANGE:"+ enemyAI.gameObject.name +" IDLE STATE");
        Debug.Log("IdleState.ENTER");
        enemyAI.movementSpeed = 0;

    }

    public override void ExitState(PatrolingEnemy enemyAI)
    {

    }

    public override void UpdateState(PatrolingEnemy enemyAI)
    {
        timeleft -= Time.deltaTime;
        if (timeleft < 0)
        {
            enemyAI.TransitionToState(enemyAI.patrolstate);
        }
    }
}
