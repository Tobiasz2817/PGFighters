using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestBinds : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;
    private void Start() {
        //_playerInput.Get

        //_playerInput.actions["Movement"].performed += (action) => Debug.Log(action.ReadValue<Vector3>());

        InputManager.Input.OnGetMovement += (ta) => Debug.Log("");
    }

    private void Update() {

    }
}

