using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyBaseState
{
    #region References
    PatrolingEnemy patrolingenemy;
    #endregion
    private int destPoint = 0;
    
    public override void EnterState(PatrolingEnemy enemyAI)
    {
        Debug.Log("STATE CHANGE:" + enemyAI.gameObject.name + " PATROL STATE");
        Debug.Log("patrolstate.ENTER");
        enemyAI.movementSpeed = 2f;
        patrolingenemy = enemyAI;
        enemyAI.agent.autoRepath = true;
    }

    public override void ExitState(PatrolingEnemy enemyAI)
    {
        patrolingenemy.agent.destination = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    public override void UpdateState(PatrolingEnemy enemyAI)
    {
        AnimationState();
        NextDestination();
        Debug.Log(patrolingenemy.gameObject.name);
    }

    public void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (patrolingenemy.points.Count == 0)
            return;

        // Set the agent to go to the currently selected destination.
        patrolingenemy.agent.destination = patrolingenemy.points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (Random.Range(0, patrolingenemy.points.Count)) % patrolingenemy.points.Count;
    }


    public void NextDestination()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (patrolingenemy.agent != null)
        {
            if (!patrolingenemy.agent.pathPending && patrolingenemy.agent.remainingDistance < 0.1f)
            {
                patrolingenemy.startroutines("WaitAtPoint");
                GotoNextPoint();
            }
            else
            {
                return;
            }
        }
    }
    void AnimationState()
    {
        if (patrolingenemy.currentspeed <= 0)
        {
            patrolingenemy.anim.SetBool("Walking", false);

        }
        else if (patrolingenemy.currentspeed > 0)
        {
            patrolingenemy.anim.SetBool("Walking", true);
        }
    }


}
