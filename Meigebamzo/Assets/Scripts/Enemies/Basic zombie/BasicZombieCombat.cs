using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BasicZombieCombat : MonoBehaviour
{
    public ComboAttack ZombieAttack => _attack;
    [SerializeField] Transform _attackTran;
    [SerializeField] float _attackRange;
    [SerializeField] LayerMask attacklayer;
    [SerializeField] ComboAttack _attack;
    List<Collider2D> hitObjects = new List<Collider2D>();
    DamageInfo _damageInfo;
    private int _damage;
    public void SetAttackDamage(int dmg)
    {
        _damage = dmg;
    }
    public void Attack()
    {
        _damageInfo = new DamageInfo(_damage, transform.position, GlobalBasicElements.PhysicalElement);
        hitObjects= Physics2D.OverlapCircleAll(_attackTran.position, _attackRange, attacklayer).ToList();
        foreach(Collider2D col in hitObjects)
        {
            if (col.attachedRigidbody)
            {
                IDamagable damageable = col.attachedRigidbody.GetComponentInParent<IDamagable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(_damageInfo);
                }
            }
        }
       // Logger.Log("Attak");
    }

    private void OnDrawGizmosSelected()
    {
        if(_attackTran) Gizmos.DrawWireSphere(_attackTran.position, _attackRange);
    }
}
