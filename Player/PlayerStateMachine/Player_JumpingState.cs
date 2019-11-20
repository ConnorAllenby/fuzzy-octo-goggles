using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_JumpingState : PlayerBaseState
{
    PlayerController playerRef;

    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;

    private Vector3 moveDirection = Vector3.zero;
    public override void EnterState(PlayerController player)
    {
        Debug.Log("Player State  = Jumping STATE");
        playerRef = player;



    }

    public override void ExitState(PlayerController player)
    {
    }

    public override void UpdateState(PlayerController player)
    {

        if (Input.GetButton("Jump"))
        {
            player.speed.y = jumpSpeed;
        }
        

        player.speed.y -= gravity * Time.deltaTime;
        player.characterController.Move(player.speed * Time.deltaTime);



        if (player.characterController.isGrounded)
        {
            player.TransitionToState(player.playerRunningState);
        }
    }


}
