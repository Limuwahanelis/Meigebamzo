using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovingState : PlayerState
{
    public static Type StateType { get => typeof(PlayerMovingState); }
    private Vector2 _positionTomoveTo;
    public PlayerMovingState(GetState function) : base(function)
    {
    }

    public override void Update()
    {
        PerformInputCommand();
        if(Vector2.Distance( _context.playerMovement.PlayerRB.position,_context.playerMovement.TargetPosition)>0.01f)
        {
            _context.playerMovement.PlayerRB.MovePosition(Vector2.MoveTowards(_context.playerMovement.PlayerRB.position, _context.playerMovement.TargetPosition, _context.playerMovement.Speed * Time.fixedDeltaTime));
        }
        else ChangeState(PlayerIdleState.StateType);
    }

    public override void SetUpState(PlayerContext context)
    {
        base.SetUpState(context);
    }
    public override void Move(Vector2 point)
    {
        _context.playerMovement.SetPositionToMoveTo(point);
       
    }
    public override void InterruptState()
    {
    
    }
}