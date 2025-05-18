using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public Rigidbody2D EnemyRB => _rb;
    [SerializeField] protected HealthSystem _healthSystem;
    [SerializeField] protected Transform _mainBody;
    [SerializeField] protected Transform _playerMainBody;
    [SerializeField] protected Rigidbody2D _rb;
    [SerializeField] protected ElementalAffliction _elementalAffliction;
}
