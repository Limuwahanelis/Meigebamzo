using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerSpells : MonoBehaviour
{
    [SerializeField] List<PlayerElementalSpells> _availableElementalSpells;
    [SerializeField] int _maxNumberOfSelectedElements;
    [SerializeField] Transform _mainBody;
    private List<PlayerElementalSpells> _selectedElements = new List<PlayerElementalSpells>();

    [Header("Electricity")]
    [Tooltip("Spread in degrees"),SerializeField] float _spread=2f;
    [SerializeField] SpritePool _thunderSpritePool;
    [SerializeField] float _thunderDuration=0.1f;
    [SerializeField] float _thunderCooldown = 0.5f;
    [SerializeField] Transform _thunderAttackDetection;
    [SerializeField] ParticleSystem _thunderParticlesPrefab;
    [SerializeField] int _count;
    [SerializeField] float _startLifetime;
    [SerializeField] float _length;
    [SerializeField] List<ParticleSystem> _paritcles= new List<ParticleSystem>();
    [Header("Fire")]
    [SerializeField] float _fireRange;
    [SerializeField] float _fireAngle;
    [SerializeField] Transform _fireEndTran;
    [SerializeField] Transform _testTran;
    [SerializeField] ParticleSystem _fireParticleSystem;
    [SerializeField] PolygonCollider2D _fireTrigger;
    [SerializeField] int _fireAttackDamage;
    [SerializeField] float _fireAttackCooldown;
    private List<float> angles = new List<float>() {0,0 };
    private bool _canAttackElectricity = true;
    private ContinousAttack _cutrrentContinousAttack;
    private List<Vector2> _fireTriangle;
    List<IDamagable> _damageablesInRange = new List<IDamagable>();
    Dictionary<Elements.Element, ContinousAttack> _continousAttacks= new Dictionary<Elements.Element, ContinousAttack>();
    private void Awake()
    {
        _continousAttacks.Add(Elements.Element.FIRE, new FireAttack(this,_fireParticleSystem, _damageablesInRange, _fireTrigger, _mainBody, _fireRange, _fireAngle,_fireAttackDamage,_fireAttackCooldown));
        _continousAttacks.Add(Elements.Element.ELECTRICITY, new ElectricityAttack(_mainBody, _spread, _paritcles, _damageablesInRange, angles, this, _thunderParticlesPrefab));
    }
    private void Update()
    {
        _thunderAttackDetection.up = (RaycastFromCamera2D.MouseInWorldPos-_mainBody.position ).normalized;

    }
    public void SetEnemyForAttack(GameObject enemy)
    {
        _damageablesInRange.Add(enemy.GetComponent<IDamagable>());
        enemy.GetComponent<HealthSystem>().OnDeath += OnEnemyDied;
        Logger.Log("ADDed");
    }
    public void RemoveEnemyFromAttack(GameObject enemy)
    {
        _damageablesInRange.Remove(enemy.GetComponent<IDamagable>());
        enemy.GetComponent<HealthSystem>().OnDeath -= OnEnemyDied;
        Logger.Log("Removed");
    }
    private void OnEnemyDied(IDamagable damagable)
    {
        damagable.OnDeath -= OnEnemyDied;
        _damageablesInRange.Remove(damagable);
    }
    private void RemoveEnemiesFromRange()
    {
        _damageablesInRange.Clear();
    }
    public void AddElement(Elements.Element element)
    {

        if(_selectedElements.Count< _maxNumberOfSelectedElements)
        {
            Logger.Log(element.ToString());
            _selectedElements.Add(_availableElementalSpells.Find(x=>x.Element==element));
        }
    }
    public void StartAttack()
    {
        _cutrrentContinousAttack = _continousAttacks[_selectedElements[0].Element];
        _cutrrentContinousAttack.StartAttack();
    }
    public void Attack()
    {
        _cutrrentContinousAttack.Attack();
    }
    public void EndAttack()
    {
        _cutrrentContinousAttack.EndAttack();
        RemoveEnemiesFromRange();
    }
  
    List<Vector2> GetTriangle()
    {
        _fireEndTran.position=_mainBody.transform.position;
        List<Vector2> toReturn = new List<Vector2>() {new Vector2(),new Vector2(),new Vector2() };
        Vector2 pointA = _mainBody.transform.position;
        Vector2 mouseDir= (RaycastFromCamera2D.MouseInWorldPos- _mainBody.transform.position).normalized;
        Vector2 fireForwardPoint=pointA+mouseDir*_fireRange;
        Vector2 fireForwardDir = (fireForwardPoint- (Vector2)_mainBody.transform.position);

        float mult=fireForwardPoint.magnitude;
        _fireEndTran.up = fireForwardDir.normalized;

        Quaternion rot= Quaternion.AngleAxis(_fireAngle / 2, Vector3.forward);
        Vector2 ABdir = rot * fireForwardDir;

        float aFunParam = ABdir.y / ABdir.x;

        rot = Quaternion.AngleAxis(-_fireAngle / 2, Vector3.forward);
        Vector2 ACdir = rot * fireForwardDir ;

        toReturn[0]=pointA+ABdir;
        toReturn[1] = pointA;
        toReturn[2] = pointA+ACdir;
        _fireEndTran.position = fireForwardPoint;
        

        return toReturn;
    }

    bool SameSide(Vector2 p1, Vector2 p2, Vector2 a, Vector2 b)
    {
        Vector3 cp1 = Vector3.Cross(b - a, p1 - a);
        Vector3 cp2 = Vector3.Cross(b - a, p2 - a);
        if (Vector3.Dot(cp1, cp2) >= 0) return true;
        else return false;
    }

    bool PointInTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
    {
        if (SameSide(p, a, b, c) && SameSide(p, b, a, c)
            && SameSide(p, c, a, b)) return true;
        else return false;
    }

    private void OnDrawGizmos()
    {
        List<Vector2> points = GetTriangle();

        Gizmos.DrawLine(points[0], points[1]);
        Gizmos.DrawLine(points[1], points[2]);
        Gizmos.DrawLine(points[0], points[2]);
        Gizmos.DrawSphere(points[1]+ (Vector2)(RaycastFromCamera2D.MouseInWorldPos - _mainBody.transform.position).normalized*_fireRange, 0.3f);
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawLine(_mainBody.position, _mainBody.position + (RaycastFromCamera2D.MouseInWorldPos - _mainBody.transform.position));
    }
}
