using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneMissile : MonoBehaviour,IDamagable
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] float _rotationSpeed;
    [SerializeField] float _speed;
    [SerializeField] LayerMask _enemyLayer;
    [SerializeField] Collider2D _col;
    private Transform _enemyTrans;
    private float _angle=0;
    private Vector2 _direction;
    private Vector2 _originPoint;

    public event IDamagable.OnDeathEventHandler OnDeath;

    public Transform Transform => transform;

    public Transform MainBody => transform;

    public ElementalAffliction ElementalAffliction => throw new System.NotImplementedException();

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetRotation(float startAngle)
    {
        _angle = startAngle;
        float impulse = (_rotationSpeed * Mathf.Deg2Rad) * _rb.inertia;
        _rb.AddTorque(impulse, ForceMode2D.Impulse);
    }
    public void SetOwner(Transform tran)
    {
        _enemyTrans = tran;
    }
    public void SetDirectionAndSpeed(Vector2 target,float speed) 
    {
        float impulse = (_rotationSpeed * Mathf.Deg2Rad) * _rb.inertia;
        _rb.AddTorque(impulse, ForceMode2D.Impulse);
        _direction = (target - _rb.position).normalized;
        _speed = speed;
        _rb.AddForce(_direction * _speed,ForceMode2D.Impulse);
    }
    private void Update()
    {
        if (_rb.angularVelocity < 40f) gameObject.SetActive(false);

    }

    public void TakeDamage(DamageInfo info)
    {

    }

    public void Kill(DamageInfo info)
    {
        
    }
}
