using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_SprintState : PlayerBaseState
{
    PlayerController playerRef;

    public override void EnterState(PlayerController player)
    {
        Debug.Log("Player State = Sprinting");
    }
    public override void ExitState(PlayerController player)
    {
    }
    public override void UpdateState(PlayerController player)
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.W))
        {

            player.movementSpeed = player.sprintSpeed;
            player.Movement();
        }
        else
        {
            player.TransitionToState(player.playerRunningState);

        }

    }
}
