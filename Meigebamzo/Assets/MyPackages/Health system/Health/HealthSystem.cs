﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PlayerHealthSystem;

[RequireComponent(typeof(ElementalAffliction))]
public class HealthSystem : MonoBehaviour,IDamagable
{
    public event IDamagable.OnDeathEventHandler OnDeath;
    public Action<DamageInfo> OnHitEvent;
    public bool IsAlive => _currentHP > 0;
    public int CurrentHP => _currentHP;
    public int MaxHP => _maxHP;

    public Transform Transform => transform;
   
    public ElementalAffliction ElementalAffliction => _elementalAffliction;

    public Transform MainBody => _mainBody ? _mainBody : transform;

    [SerializeField] protected Collider2D[] _colliders;
    [SerializeField] protected bool _isInvincible;
    [SerializeField] protected HealthBar _hpBar;
    [SerializeField] protected int _maxHP;
    [SerializeField] protected int _currentHP;
    [SerializeField] float _invincibilityAfterHitDuration;
    [SerializeField] protected ElementalAffliction _elementalAffliction;
    [SerializeField] protected Transform _mainBody;
    protected bool _isInvincibleToDamage = false;




    // Start is called before the first frame update
    protected void Start()
    {
        _hpBar.SetMaxHealth(_maxHP);
        _currentHP = _maxHP;
        _hpBar.SetHealth(_currentHP);
    }
    public void SetMacHP(int value)
    {
        _maxHP = value;
    }
    public virtual void TakeDamage(DamageInfo info)
    {
        if (_isInvincibleToDamage) return;
        if (!IsAlive) return;
        _currentHP -= info.dmg;
        if (_hpBar != null) _hpBar.SetHealth(_currentHP);
        OnHitEvent?.Invoke(info);
        StartCoroutine(DamageInvincibilityCor());
        if (_currentHP <= 0) Kill(info);
    }
    /// <summary>
    /// Deals damage wihtout rasing any events.
    /// </summary>
    /// <param name="info"></param>
    public virtual void TakeDamageWithoutNotify(DamageInfo info)
    {
        if (!IsAlive) return;
        _currentHP -= info.dmg;
        _hpBar.SetHealth(_currentHP);
        if (_currentHP <= 0) Kill(info);
    }
    protected bool IsDeathEventSubscribedTo()
    {
        if (OnDeath == null) return false;
         return true;
    }
    protected void InvokeOnDeathEvent(DamageInfo info)
    {
        OnDeath?.Invoke(this, info);
    }
    public virtual void Kill(DamageInfo info)
    {
        if (!IsDeathEventSubscribedTo())
        {
            Destroy(gameObject);
            Destroy(_hpBar.gameObject);
        }
        else OnDeath?.Invoke(this, info);
    }
    IEnumerator DamageInvincibilityCor()
    {
        _isInvincibleToDamage = true;
        yield return new WaitForSeconds(_invincibilityAfterHitDuration);
        _isInvincibleToDamage = false;
    }
    private void Reset()
    {
        _elementalAffliction = GetComponent<ElementalAffliction>();
    }
    #region collisions
    protected void PreventCollisions(Collider2D[] collidersToPreventCollisionsFrom)
    {
        foreach(Collider2D collider in collidersToPreventCollisionsFrom)
        {
            foreach(Collider2D myCol in _colliders)
            {
                Physics2D.IgnoreCollision(collider, myCol, true);
            }
        }
    }

    protected void RestoreCollisions(Collider2D[] collidersToRestoreCollisionsFrom)
    {
        foreach (Collider2D collider in collidersToRestoreCollisionsFrom)
        {
            foreach (Collider2D myCol in _colliders)
            {
                Physics2D.IgnoreCollision(collider, myCol, false);
            }
        }
    }
    #endregion
}
