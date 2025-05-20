using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperStateCasting : EnemyState
{
    public static Type StateType { get => typeof(ReaperStateCasting); }
    private ReaperContext _context;
    private float _animLength;
    private float _time=0;
    public ReaperStateCasting(GetState function) : base(function)
    {
    }

    public override void Update()
    {
        _time += Time.deltaTime;
        if(_time>_animLength)
        {
            ChangeState(ReaperStateAttackingPlayer.StateType);
        }
    }

    public override void SetUpState(EnemyContext context)
    {
        base.SetUpState(context);
        _context = (ReaperContext)context;
        _context.animMan.PlayAnimation("Talk cast");
        _animLength = _context.animMan.GetAnimationLength("Talk cast");
    }

    public override void InterruptState()
    {
     
    }
}