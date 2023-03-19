using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public InputManager Instance;
    private void Awake() {
        Instance = this;
    }
    
    
}
