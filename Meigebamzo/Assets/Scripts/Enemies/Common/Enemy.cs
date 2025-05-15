using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public Rigidbody2D EnemyRB => _rb;
    [SerializeField] HealthSystem _healthSystem;
    [SerializeField] Transform _mainBody;
    [SerializeField] Transform _playerMainBody;
    [SerializeField] Rigidbody2D _rb;
}
