using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicZombieStateRiseFromDead : EnemyState
{
    public static Type StateType { get => typeof(BasicZombieStateRiseFromDead); }
    private BasicZombieContext _context;
    public BasicZombieStateRiseFromDead(GetState function) : base(function)
    {
    }

    public override void Update()
    {

    }

    public override void SetUpState(EnemyContext context)
    {
        base.SetUpState(context);
        _context = (BasicZombieContext)context;
        _context.animMan.PlayAnimation("Rise from dead");
        _context.coroutineHolder.StartCoroutine(HelperClass.DelayedFunction(_context.animMan.GetAnimationLength("Rise from dead"), () => { ChangeState(BasicZombieStateIdle.StateType); }));
    }

    public override void InterruptState()
    {
     
    }
}