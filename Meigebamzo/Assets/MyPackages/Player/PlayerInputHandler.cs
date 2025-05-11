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
    private Vector2 _direction;
    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void OnMousePos(InputValue inputValue)
    {
        HelperClass.SetMousePos(inputValue.Get<Vector2>());
    }
    protected void OnClick()
    {
       _inputStack.CurrentCommand=new MoveInputCommand(_player.CurrentPlayerState,RaycastFromCamera2D.MouseInWorldPos );
    }
}
