using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAttack : ContinousAttack
{
    PlayerSpells _playerSpells;
    ParticleSystem _particleSystem;
    PolygonCollider2D _fireTrigger;
    Transform _mainBody;
     float _fireRange;
    float _fireAngle;
    private int _attackDamage;
    private float _attackCooldown;
    private DamageInfo _damageInfo;
    bool _canDealDamage = true;
    ParticleSystem.EmitParams _params;
    
    public FireAttack(PlayerSpells playerSpells, ParticleSystem system,List<IDamagable> damageables,PolygonCollider2D fireTrigger,Transform mainBody, float fireRange,float fireAngle, int attackDamage,float attackCooldown) 
    {
        _playerSpells = playerSpells;
        _particleSystem = system;
        _damageablesInRange = damageables;
        _fireTrigger = fireTrigger;
        _mainBody = mainBody;
        _fireRange = fireRange;
        _fireAngle = fireAngle;
        _attackDamage = attackDamage;
        _attackCooldown = attackCooldown;
        _params = new ParticleSystem.EmitParams();
    }
    public override void Attack()
    {
        Vector2 direction = RaycastFromCamera2D.MouseInWorldPos - _mainBody.position;
        _particleSystem.transform.up = direction.normalized;
        ParticleSystem.MainModule mainModule = _particleSystem.main;
        ParticleSystem.MinMaxCurve curve= mainModule.startRotation;
        curve.mode = ParticleSystemCurveMode.TwoConstants;
        float radians = -_particleSystem.transform.rotation.eulerAngles.z * Mathf.PI / 180f;
        float spreadRad = 15f * Mathf.PI / 180f;
        curve.constant = radians;
        curve.constantMin = radians - spreadRad;
        curve.constantMax = radians + spreadRad;
        mainModule.startRotation = curve;
        List<Vector2> _trianglePoints = GetTriangle();
        List<Vector2> tmp = new List<Vector2>() { _mainBody.transform.InverseTransformPoint(_trianglePoints[0]), _mainBody.transform.InverseTransformPoint(_trianglePoints[1]), _mainBody.transform.InverseTransformPoint(_trianglePoints[2]) };
        _fireTrigger.points = tmp.ToArray();
        _playerSpells.StartCoroutine(DamageCor());
    }

    public override void EndAttack()
    {
        _particleSystem.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        _fireTrigger.enabled = false;
        _spells.Clear();
    }

    public override void StartAttack()
    {
        _particleSystem.transform.position= _mainBody.transform.position;
        _particleSystem.Play();
        _fireTrigger.enabled = true;
    }

    List<Vector2> GetTriangle()
    {
        List<Vector2> toReturn = new List<Vector2>() { new Vector2(), new Vector2(), new Vector2() };
        Vector2 pointA = _mainBody.transform.position;
        Vector2 mouseDir = (RaycastFromCamera2D.MouseInWorldPos - _mainBody.transform.position).normalized;
        Vector2 fireForwardPoint = pointA + mouseDir * _fireRange;
        Vector2 fireForwardDir = (fireForwardPoint - (Vector2)_mainBody.transform.position);

        float mult = fireForwardPoint.magnitude;

        Quaternion rot = Quaternion.AngleAxis(_fireAngle / 2, Vector3.forward);
        Vector2 ABdir = rot * fireForwardDir;

        float aFunParam = ABdir.y / ABdir.x;

        rot = Quaternion.AngleAxis(-_fireAngle / 2, Vector3.forward);
        Vector2 ACdir = rot * fireForwardDir;

        toReturn[0] = pointA + ABdir;
        toReturn[1] = pointA;
        toReturn[2] = pointA + ACdir;


        return toReturn;
    }

    IEnumerator DamageCor()
    {
        if(!_canDealDamage) yield break;
        Logger.Log(_spells.Count);
        _damageInfo.element = Elements.Element.FIRE;
        _damageInfo.dmgPosition= _mainBody.transform.position;
        _damageInfo.dmg = _attackDamage;
        for(int i=0;i< _damageablesInRange.Count;i++)
        {
            _damageablesInRange[i].TakeDamage(_damageInfo);
        }
        _canDealDamage = false;
        yield return new WaitForSeconds(_attackCooldown);
        _canDealDamage=true;
    }
}
