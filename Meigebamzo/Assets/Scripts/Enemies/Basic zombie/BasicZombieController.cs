using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class BasicZombieController : EnemyController
{
    [SerializeField] float _distanceToStartChase;
    [SerializeField] float _distanceToEndChase;
    [SerializeField] BasicZombieCombat _combat;
    private BasicZombieContext _context;
    public override void Awake()
    {
        base.Awake();
        Initialize();
    }
    private void Initialize()
    {
        List<Type> states = AppDomain.CurrentDomain.GetAssemblies().SelectMany(domainAssembly => domainAssembly.GetTypes())
            .Where(type => typeof(EnemyState).IsAssignableFrom(type) && !type.IsAbstract).ToArray().ToList();

        _context = new BasicZombieContext
        {
            ChangeEnemyState=ChangeState,
            animMan=_enemyAnimationManager,
            playerTransform=_playerTransform,
            enemyTransform=transform,
            stats=_enemyBasicStats,
            distanceToStartChase=_distanceToStartChase,
            distanceToEndChase=_distanceToEndChase,
            combat=_combat,
            coroutineHolder=this
        };

        EnemyState.GetState getState = GetState;
        foreach (Type state in states)
        {
            _enemyStates.Add(state, (EnemyState)Activator.CreateInstance(state, getState));
        }
        //Set Startitng state
        EnemyState newState = GetState(typeof(BasicZombieStateRiseFromDead));
        newState.SetUpState(_context);
        _currentEnemyState = newState;
        Logger.Log(newState.GetType());
    }
    private void Start()
    {
        
    }
}
