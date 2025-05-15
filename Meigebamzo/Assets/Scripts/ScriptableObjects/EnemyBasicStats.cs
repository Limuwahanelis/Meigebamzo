using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Stats/EnemyBasicStats")]
public class EnemyBasicStats : ScriptableObject
{

    public float Speed { get => _speed; }
    public int Damage { get => _damage; }
    public int MaxHP { get => _maxHP; }

    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private int _maxHP;

}