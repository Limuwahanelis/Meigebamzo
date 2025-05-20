using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperStateAttackingPlayer : EnemyState
{
    public static Type StateType { get => typeof(ReaperStateAttackingPlayer); }
    private ReaperContext _context;
    private bool _isPerformingAttack;
    private float _attackTime;
    private bool _shouldDealdamage = true;
    private float _time = 0;
    private int _spawnIndex;
    private bool _spawnedFirstBones;

    public ReaperStateAttackingPlayer(GetState function) : base(function)
    {
    }

    public override void Update()
    {
        _context.boneAttackTime += Time.deltaTime;
        if (_context.boneAttackTime > _context.boneMissileCooldown)
        {
            _context.combat.SpawnBone(_spawnIndex, _context.playerTransform, _context.boneSpeed + UnityEngine.Random.Range(-0.25f, 0.25f));
            _spawnIndex++;
            if (_spawnIndex > 1) _spawnIndex = 0;
            _context.boneAttackTime = 0;
        }
    }

    public override void SetUpState(EnemyContext context)
    {
        base.SetUpState(context);
        _context = (ReaperContext)context;
        _context.animMan.PlayAnimation("Idle cast");
        _context.combat.SpawnObject(0, _context.enemyTransform);
        _context.combat.SpawnObject(1, _context.enemyTransform);
        _spawnedFirstBones = true;
    }

    public override void InterruptState()
    {
     
    }
}