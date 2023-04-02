using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

public class InputManager
{
    public static BaseInput Input;

    [RuntimeInitializeOnLoadMethod]
    private void Awake() {
        Input = new StandardInputs();
    }
}
public abstract class BaseInput
{
    public Action<InputAction.CallbackContext> OnGetMovement;
    public Action<InputAction.CallbackContext> OnGetMouseDelta;
    public Action<InputAction.CallbackContext> OnGetMousePosition;
    public Action<InputAction.CallbackContext> OnLeftMouseButtonPressed;
    public Action<InputAction.CallbackContext> OnRightMouseButtonPressed;
    
    public Inputs CurrentInput;
    protected InputState currentState = InputState.Gameplay;

    private Dictionary<InputState, Inputs> Inputs = new Dictionary<InputState, Inputs>();

    protected BaseInput() {
        CurrentInput = new Inputs();
        CurrentInput.Enable();
        Debug.Log("Enable Input Action");
    }

    public enum InputState
    {
        UI,
        GameUI,
        Gameplay
    }
}

public class StandardInputs : BaseInput
{
    public StandardInputs() {
        var GameplayInput = new Inputs();
        GameplayInput.KeyboardCharacter.Movement.performed += OnGetMovement;
        
    }
}