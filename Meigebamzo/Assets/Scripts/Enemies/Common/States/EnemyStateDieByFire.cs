using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateDieByFire : EnemyState
{
    public static Type StateType { get => typeof(EnemyStateDieByFire); }
    private EnemyContext _context;
    private float t = 0;
    private float burnPct = 0;
    public EnemyStateDieByFire(GetState function) : base(function)
    {
    }

    public override void Update()
    {
        burnPct = t / _context.timeToBurn;
        _context.spriteRenderer.material.SetFloat("_Fade", burnPct);
        Logger.Log(burnPct);
        t += Time.deltaTime;
        if(burnPct > 1) _context.enemyGameobject.SetActive(false);
    }

    public override void SetUpState(EnemyContext context)
    {
        base.SetUpState(context);
        _context = (EnemyContext)context;
        context.spriteRenderer.material = context.burnDeathMat;
        context.animMan.PlayAnimation("Idle");
    }

    public override void InterruptState()
    {
     
    }
}