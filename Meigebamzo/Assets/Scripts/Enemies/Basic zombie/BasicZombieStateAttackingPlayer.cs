using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicZombieStateAttackingPlayer : EnemyState
{
    public static Type StateType { get => typeof(BasicZombieStateAttackingPlayer); }
    private BasicZombieContext _context;
    private bool _isPerformingAttack;
    private float _attackTime;
    private float _time = 0;
    public BasicZombieStateAttackingPlayer(GetState function) : base(function)
    {
    }

    public override void Update()
    {
        if (!_isPerformingAttack)
        {
            if (Vector2.Distance(_context.enemyTransform.position, _context.playerTransform.position) > _context.distanceToStartChase)
            {
                ChangeState(BasicZombieStateIdle.StateType);
            }
            else 
            {
                ChangeState(BasicZombieStateIdle.StateType);
            }
        }
        _time += Time.deltaTime;
        if(_time>=_attackTime)
        {
            _time = 0;
            _isPerformingAttack = false;
            _context.combat.Attack();
            ChangeState(BasicZombieStateIdle.StateType);
        }
    }

    public override void SetUpState(EnemyContext context)
    {
        base.SetUpState(context);
        _context = (BasicZombieContext)context;
        _time = 0;
        _isPerformingAttack = true;
        _attackTime = _context.animMan.GetAnimationLength("Attack");
        _context.animMan.PlayAnimation("Attack");
    }

    public override void InterruptState()
    {
     
    }
}