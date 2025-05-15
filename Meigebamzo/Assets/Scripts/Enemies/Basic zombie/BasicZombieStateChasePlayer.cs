using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicZombieStateChasePlayer : EnemyState
{
    public static Type StateType { get => typeof(BasicZombieStateChasePlayer); }
    private BasicZombieContext _context;
    public BasicZombieStateChasePlayer(GetState function) : base(function)
    {
    }

    public override void Update()
    {
        if(Vector2.Distance(_context.enemyTransform.position,_context.playerTransform.position)>_context.distanceToStartChase)
        {
           _context.enemyTransform.position= Vector2.MoveTowards(_context.enemyTransform.position, _context.playerTransform.position, Time.deltaTime * _context.stats.Speed);
        }
        else
        {
            ChangeState(BasicZombieStateAttackingPlayer.StateType);
        }
    }

    public override void SetUpState(EnemyContext context)
    {
        base.SetUpState(context);
        _context = (BasicZombieContext)context;
    }

    public override void InterruptState()
    {
     
    }
}