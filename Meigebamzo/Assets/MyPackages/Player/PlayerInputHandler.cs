using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{

    [SerializeField] PlayerController _player;
    [SerializeField] InputActionAsset _controls;
    [SerializeField] PlayerInputStack _inputStack;
    [SerializeField] PlayerSpells _spells;
    private Vector2 _direction;
    private bool _isHoldingRMB = false;
    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isHoldingRMB)
        {
            _spells.Attack();
            //_spells.ElectricityAttack();
        }
    }
    public void OnMousePos(InputValue inputValue)
    {
        HelperClass.SetMousePos(inputValue.Get<Vector2>());
    }
    protected void OnClick()
    {
       _inputStack.CurrentCommand=new MoveInputCommand(_player.CurrentPlayerState,RaycastFromCamera2D.MouseInWorldPos );
    }
    protected void OnElementSelect(InputValue inputValue)
    {
        if (inputValue.Get<float>() == 0) return;
        //Logger.Log(inputValue.Get<float>());
        _spells.AddElement((Elements.Element)inputValue.Get<float>());
    }
    protected void OnRMB(InputValue inputValue)
    {
        _inputStack.CurrentCommand = new MoveInputCommand(_player.CurrentPlayerState, _player.MainBody.transform.position);
        if (inputValue.Get<float>() >= 1)
        {
            if(_spells.StartAttack())
            {

                _isHoldingRMB = true;
            }
        }
        else
        {
            if(_isHoldingRMB)
            {
                _spells.EndAttack();
                _isHoldingRMB = false;
            }
        }

        
    }
}
