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
        _healthSystem.OnDeath += OnDeath;
    }
    private void Initialize()
    {
        List<Type> states = AppDomain.CurrentDomain.GetAssemblies().SelectMany(domainAssembly => domainAssembly.GetTypes())
            .Where(type => typeof(EnemyState).IsAssignableFrom(type) && !type.IsAbstract).ToArray().ToList();

        _context = new BasicZombieContext
        {
            ChangeEnemyState = ChangeState,
            animMan = _enemyAnimationManager,
            playerTransform = _playerTransform,
            enemyTransform = _mainBody.transform,
            stats = _enemyBasicStats,
            distanceToStartChase = _enemyBasicStats.DistanceToStartChase,
            distanceToEndChase = _enemyBasicStats.DistanceToEndChase,
            combat = _combat,
            enemyRigidBody2D = _rb,
            playerRB = _playerRB,
            enemyGameobject=gameObject,
            burnDeathMat= _burnMaterial,
            timeToBurn= _timeToBurn,
            spriteRenderer=_spriteRenderer,
            coroutineHolder = this
        };

        EnemyState.GetState getState = GetState;
        foreach (Type state in states)
        {
            _enemyStates.Add(state, (EnemyState)Activator.CreateInstance(state, getState));
        }
        _combat.SetAttackDamage(_enemyBasicStats.Damage);
        //Set Startitng state
        EnemyState newState = GetState(typeof(BasicZombieStateRiseFromDead));
        newState.SetUpState(_context);
        _currentEnemyState = newState;
        Logger.Log(newState.GetType());
    }
    private void Start()
    {

    }
    public override void OnDeath(IDamagable damagable, DamageInfo info)
    {
        base.OnDeath(damagable,info);
        _currentEnemyState.SetUpState(_context);
        AllEnemiesList.RemoveEnemy(_healthSystem);
        //gameObject.SetActive(false);
        OnEnemyDied?.Invoke();
    }
}
