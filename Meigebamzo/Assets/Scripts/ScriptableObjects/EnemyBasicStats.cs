using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Stats/EnemyBasicStats")]
public class EnemyBasicStats : ScriptableObject
{

    public float Speed { get => _speed; }
    public int Damage { get => _damage; }
    public int MaxHP { get => _maxHP; }
    public float DistanceToStartChase { get => _distanceToStartChase;}
    public float DistanceToEndChase { get => _distanceToEndChase; }

    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private int _maxHP;
    [SerializeField] private float _distanceToStartChase;
    [SerializeField] private float _distanceToEndChase;
}