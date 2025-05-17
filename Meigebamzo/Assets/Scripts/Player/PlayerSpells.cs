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
    [SerializeField] List<SpriteRenderer> _spellSlotsRenderes = new List<SpriteRenderer>();
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
    [SerializeField] BoxCollider2D _electricityTrigger;
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
    [Header("Wind")]
    [SerializeField] List<Transform> _windPushTrans= new List<Transform>();
    [SerializeField] List<GameObject> _windPushes= new List<GameObject>();
    
    private List<float> angles = new List<float>() {0,0 };
    private bool _canAttackElectricity = true;
    private ContinousAttack _cutrrentContinousAttack;
    private List<Vector2> _fireTriangle;
    List<IDamagable> _damageablesInRange = new List<IDamagable>();
    Dictionary<Elements.Element, ContinousAttack> _continousAttacks= new Dictionary<Elements.Element, ContinousAttack>();
    private void Awake()
    {
        foreach (SpriteRenderer spriteRenderer in _spellSlotsRenderes)
        {
            spriteRenderer.gameObject.SetActive(false);
        }
        _continousAttacks.Add(Elements.Element.FIRE, new FireAttack(this,_fireParticleSystem, _damageablesInRange, _fireTrigger, _mainBody, _fireRange, _fireAngle,_fireAttackDamage,_fireAttackCooldown));
        _continousAttacks.Add(Elements.Element.ELECTRICITY, new ElectricityAttack(_mainBody, _spread, _paritcles, _damageablesInRange, angles, this, _thunderParticlesPrefab, _electricityTrigger));
    }
    private void Update()
    {
        _thunderAttackDetection.up = (RaycastFromCamera2D.MouseInWorldPos-_mainBody.position ).normalized;

    }
    public void SetEnemyForAttack(GameObject enemy)
    {
        _damageablesInRange.Add(enemy.GetComponent<IDamagable>());
        enemy.GetComponent<IDamagable>().OnDeath += OnEnemyDied;
        Logger.Log("ADDed");
    }
    public void RemoveEnemyFromAttack(GameObject enemy)
    {
        _damageablesInRange.Remove(enemy.GetComponent<IDamagable>());
        enemy.GetComponent<IDamagable>().OnDeath -= OnEnemyDied;
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
            //Logger.Log(element.ToString());
            PlayerElementalSpells spell = _availableElementalSpells.Find(x => x.Element == element);
            _selectedElements.Add(spell);
            _spellSlotsRenderes[_selectedElements.Count - 1].sprite = spell.Sprite;
            _spellSlotsRenderes[_selectedElements.Count - 1].gameObject.SetActive(true);
        }
    }
    private Elements.Element DetermineAttack()
    {
        if(_selectedElements.Find(x=>x.Element==Elements.Element.ELECTRICITY)) return Elements.Element.ELECTRICITY;
        else if(_selectedElements.Find(x=>x.Element==Elements.Element.FIRE)) return Elements.Element.FIRE;
        else if (_selectedElements.Find(x=>x.Element==Elements.Element.WIND)) return Elements.Element.WIND;
        return Elements.Element.PHYSICAL;
    }
    public bool StartAttack()
    {
        Elements.Element attackElement = DetermineAttack();
        if (_selectedElements.Count == 0) return false;
        if (attackElement == Elements.Element.PHYSICAL)
        {
            _selectedElements.Clear();
            foreach(SpriteRenderer spriteRenderer in _spellSlotsRenderes)
            {
                spriteRenderer.gameObject.SetActive(false);
            }
            return false;
        }
        foreach (SpriteRenderer spriteRenderer in _spellSlotsRenderes)
        {
            spriteRenderer.gameObject.SetActive(false);
        }

        if (_continousAttacks.TryGetValue(attackElement, out _))
        {
            _cutrrentContinousAttack = _continousAttacks[attackElement];

            _cutrrentContinousAttack.SetSpells(_selectedElements);
            _selectedElements.Clear();
            _cutrrentContinousAttack.StartAttack();
            return true;
        }

        if(attackElement==Elements.Element.WIND)
        {
            int windForce = _selectedElements.FindAll(x => x.Element == Elements.Element.WIND).Count-1;

            _windPushes[windForce].transform.position= _windPushTrans[windForce].transform.position;
            _windPushes[windForce].transform.up = _electricityTrigger.transform.up;
            _windPushes[windForce].transform.Rotate(_windPushes[windForce].transform.forward, 45f);
            _windPushes[windForce].GetComponent<Animator>().SetInteger("PushType",windForce);
            _windPushes[windForce].GetComponent<Animator>().SetTrigger("Push");
            _selectedElements.Clear();
        }

        return false;
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
