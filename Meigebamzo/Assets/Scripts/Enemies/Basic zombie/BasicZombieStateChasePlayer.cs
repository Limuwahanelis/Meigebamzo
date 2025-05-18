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
        if(_context.enemyRigidBody2D.position.x-_context.playerRB.position.x>0) _context.enemyTransform.localScale = new Vector3(-1, 1, 1);
        else _context.enemyTransform.localScale = new Vector3(1, 1, 1);
        base.FixedUpdate();
        if (Vector2.Distance(_context.enemyRigidBody2D.position, _context.playerRB.position) > _context.distanceToStartChase)
        {
            Logger.Log(Vector2.MoveTowards(_context.enemyRigidBody2D.position, _context.playerTransform.position, Time.fixedDeltaTime * _context.stats.Speed));
            _context.enemyTransform.position=Vector2.MoveTowards(_context.enemyRigidBody2D.position, _context.playerRB.position, Time.fixedDeltaTime * _context.stats.Speed);
        }
        else
        {
            _context.enemyRigidBody2D.linearVelocity = Vector2.zero;
            ChangeState(BasicZombieStateAttackingPlayer.StateType);
        }
    }
    public override void FixedUpdate()
    {

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