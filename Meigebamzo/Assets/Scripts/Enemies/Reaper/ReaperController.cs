using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class ReaperController : EnemyController
{
    [SerializeField] float _distanceToStartChase;
    [SerializeField] float _distanceToEndChase;
    [SerializeField] ReaperCombat _combat;
    private ReaperContext _context;
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

        _context = new ReaperContext
        {
            ChangeEnemyState = ChangeState,
            animMan = _enemyAnimationManager,
            playerTransform = _playerTransform,
            enemyTransform = _mainBody.transform,
            stats = _enemyBasicStats,
            distanceToStartChase = _enemyBasicStats.DistanceToStartChase,
            combat = _combat,
            enemyRigidBody2D = _rb,
            playerRB = _playerRB,
            boneMissileCooldown = 2f,
            boneSpeed = 5f,
            coroutineHolder = this
        };

        EnemyState.GetState getState = GetState;
        foreach (Type state in states)
        {
            _enemyStates.Add(state, (EnemyState)Activator.CreateInstance(state, getState));
        }
        //Set Startitng state
        EnemyState newState = GetState(typeof(ReaperStateIdle));
        newState.SetUpState(_context);
        _currentEnemyState = newState;
        Logger.Log(newState.GetType());
    }
    private void Start()
    {

    }
    private void OnDeath(IDamagable damagable)
    {
        AllEnemiesList.RemoveEnemy(_healthSystem);
        gameObject.SetActive(false);
        OnEnemyDied?.Invoke();
    }
}
