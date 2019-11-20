using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_RunningState : PlayerBaseState
{
    PlayerController playerRef;

    // Timer
    float timeleft;
    public override void EnterState(PlayerController player)
    {
        timeleft = 0;
        Debug.Log("Player State  = RUNNING STATE");
    }
    public override void ExitState(PlayerController player)
    {

    }
    public override void UpdateState(PlayerController player)
    {
        
        Debug.Log(timeleft);

        #region Jump
        if (Input.GetButton("Jump"))
        {
            player.TransitionToState(player.playerJumpingState);
        }
        #endregion

        #region RunningLogic
        // Movement Logic
        player.sideSpeed = Input.GetAxis("Horizontal") * player.movementSpeed;
        player.forwardSpeed = Input.GetAxis("Vertical") * player.movementSpeed;
        player.speed = new Vector3(player.sideSpeed, player.verticalVelocity, player.forwardSpeed);
        player.speed = player.transform.rotation * player.speed;
        player.characterController.Move(player.speed * Time.deltaTime);

        //.

        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            // Initialize Idle t¬!imer
            timeleft += Time.deltaTime;

            if (timeleft >= 0.5f) 
            {
                player.TransitionToState(player.playerIdleState);
                Debug.Log("Yeet");
            }
        }
        #endregion

        #region Sprint
        if(Input.GetKeyDown(KeyCode.LeftShift) && player.characterController.isGrounded && Input.GetAxis("Vertical") > 0)
        {
            player.forwardSpeed = Input.GetAxis("Vertical") * player.sprintSpeed;
        }
        #endregion
    }
}
