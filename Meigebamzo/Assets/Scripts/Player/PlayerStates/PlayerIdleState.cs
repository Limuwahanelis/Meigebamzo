using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public static Type StateType { get => typeof(PlayerIdleState); }
    public PlayerIdleState(GetState function) : base(function)
    {
    }

    public override void Update()
    {
        PerformInputCommand();
    }

    public override void SetUpState(PlayerContext context)
    {
        base.SetUpState(context);
    }

    public override void Move(Vector2 point)
    {
        _context.playerMovement.SetPositionToMoveTo(point);
        ChangeState(PlayerMovingState.StateType);
    }
    public override void InterruptState()
    {
     
    }
}