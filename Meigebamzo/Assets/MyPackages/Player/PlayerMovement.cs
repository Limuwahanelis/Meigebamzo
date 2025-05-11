using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int FlipSide => _flipSide;
    public bool IsPlayerFalling { get => _rb.linearVelocity.y < 0; }
    public float Speed => _speed;
    public Rigidbody2D PlayerRB => _rb;
    public Vector2 TargetPosition=>_targetPosition;
    [Header("Common")]
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Transform _mainBody;
    [SerializeField] PlayerController _player;
    [SerializeField] float _speed;

    private int _flipSide = 1;
    private Vector2 _direction;
    private Vector2 _targetPosition;
    public void SetPositionToMoveTo(Vector2 newPosition)
    {
        _targetPosition = newPosition;
        _direction = _targetPosition-new Vector2(_mainBody.transform.position.x,_mainBody.transform.position.y);
    }
    public void Move()
    {
        if (_direction.x != 0)
        {
            _rb.linearVelocity = new Vector2(0, 0);
            _rb.MovePosition(_rb.position + new Vector2(_mainBody.right.x * _flipSide * _speed * Time.deltaTime, _mainBody.up.y*_speed*Time.deltaTime));
            if (_direction.x > 0)
            {
                _flipSide = 1;
                _player.MainBody.transform.localScale = new Vector3(_flipSide, _player.MainBody.transform.localScale.y, _player.MainBody.transform.localScale.z);
            }
            if (_direction.x < 0)
            {
                _flipSide = -1;
                _player.MainBody.transform.localScale = new Vector3(_flipSide, _player.MainBody.transform.localScale.y, _player.MainBody.transform.localScale.z);
            }
        }


    }
}
