using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicZombieStateIdle : EnemyState
{
    public static Type StateType { get => typeof(BasicZombieStateIdle); }
    private BasicZombieContext _context;
    public BasicZombieStateIdle(GetState function) : base(function)
    {
    }

    public override void Update()
    {

    }

    public override void SetUpState(EnemyContext context)
    {
        base.SetUpState(context);
        _context = (BasicZombieContext)context;
        _context.animMan.PlayAnimation("Idle");
       _context.coroutineHolder.StartCoroutine( HelperClass.DelayedFunction(1f, () => { StartChasingPlayer(); }));
    }
    private void StartChasingPlayer()
    {
        ChangeState(BasicZombieStateChasePlayer.StateType);
    }
    public override void InterruptState()
    {
     
    }
}