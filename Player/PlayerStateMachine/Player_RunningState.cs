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
        

        #region Jump
        if (Input.GetButton("Jump"))
        {
            player.TransitionToState(player.playerJumpingState);
        }
        #endregion

        #region RunningLogic
        // Movement Logic

        player.Movement();  

        //.

        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            // Initialize Idle t¬!imer
            timeleft += Time.deltaTime;

            if (timeleft >= 0.05f) 
            {
                player.TransitionToState(player.playerIdleState);
                Debug.Log("Yeet");
            }
        }
        #endregion

        #region Sprint

        #endregion
    }
}
