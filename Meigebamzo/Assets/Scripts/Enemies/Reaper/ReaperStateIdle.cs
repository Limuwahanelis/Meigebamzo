using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperStateIdle : EnemyState
{
    public static Type StateType { get => typeof(ReaperStateIdle); }
    private ReaperContext _context;
    public ReaperStateIdle(GetState function) : base(function)
    {
    }

    public override void Update()
    {

    }

    public override void SetUpState(EnemyContext context)
    {
        base.SetUpState(context);
        _context = (ReaperContext)context;
        _context.coroutineHolder.StartCoroutine(HelperClass.DelayedFunction(0.5f,() => { ChangeState(ReaperStateCasting.StateType); }));
        _context.animMan.PlayAnimation("Idle");
    }

    public override void InterruptState()
    {
     
    }
}