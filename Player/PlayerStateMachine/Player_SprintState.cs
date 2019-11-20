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
        

    }
}
