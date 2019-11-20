using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_IdleState : PlayerBaseState
{
    PlayerController playerRef;
    public override void EnterState(PlayerController player)
    {
        Debug.Log("Player State  = IDLE STATE");
        playerRef = player;



    }
    public override void ExitState(PlayerController player)
    {
    }
    public override void UpdateState(PlayerController player)
    {

        if (Input.GetButton("Jump"))
        {
            player.TransitionToState(player.playerJumpingState);
        }

        if (Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.D))
        {
            player.TransitionToState(player.playerRunningState);
        }
    }
}
