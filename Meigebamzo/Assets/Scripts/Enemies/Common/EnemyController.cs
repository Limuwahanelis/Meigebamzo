using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    public Rigidbody2D EnemyRB => _rb;
    [Header("Debug"), SerializeField] bool _printState;
    public GameObject MainBody => _mainBody;
    [Header("Enemy common"), SerializeField] protected AnimationManager _enemyAnimationManager;
    [SerializeField] protected EnemyBasicStats _enemyBasicStats;
    //[SerializeField] protected EnemyHealthSystem2 _healthSystem;
    [SerializeField] protected Transform _playerTransform;
    [SerializeField] protected GameObject _mainBody;
    [SerializeField] protected HealthSystem _healthSystem;
    [SerializeField] protected Transform _playerMainBody;
    [SerializeField] protected Rigidbody2D _rb;
    [SerializeField] protected ElementalAffliction _elementalAffliction;
    protected Dictionary<Type, EnemyState> _enemyStates = new Dictionary<Type, EnemyState>();
    protected EnemyState _currentEnemyState;
    public virtual void Awake()
    {
        _healthSystem.OnHitEvent += OnHit;
    }

    public EnemyState GetState(Type state)
    {
        return _enemyStates[state];
    }
    public virtual void Update()
    {
        _currentEnemyState.Update();
    }
    public void ChangeState(EnemyState newState)
    {
        if (_printState) Logger.Log(newState.GetType());
        _currentEnemyState.InterruptState();
        _currentEnemyState = newState;
    }
    public Coroutine WaitFrameAndExecuteFunction(Action function)
    {
        return StartCoroutine(WaitFrame(function));
    }
    public IEnumerator WaitFrame(Action function)
    {
        yield return new WaitForNextFrameUnit();
        function();
    }

    protected void OnHit(DamageInfo info)
    {
        if (info.element == Elements.Element.WATER) _elementalAffliction.TrySetElement(Elements.Element.WATER);
    }
}
