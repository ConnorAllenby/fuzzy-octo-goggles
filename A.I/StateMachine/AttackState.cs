using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyBaseState
{
    GameObject player;

    void start()
    {
        
    }
    public override void EnterState(PatrolingEnemy enemyAI)
    {
        Debug.Log("STATE CHANGE:" + enemyAI.gameObject.name + " ATTACK STATE");
        Debug.Log("AttackState.ENTER");
        enemyAI.movementSpeed = 0;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void ExitState(PatrolingEnemy enemyAI)
    {

    }

    public override void UpdateState(PatrolingEnemy enemyAI)
    {
        enemyAI.agent.SetDestination(player.transform.position);
    }
}
