using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllStateInputs : MonoBehaviour
{
    private void Awake() {
        GameManager.OnGameStarted += StartGame;
        GameManager.OnGameOverHandler += EndGame;
        GameManager.OnGamePause += PauseGame;
    }

    private void OnDestroy() {
        GameManager.OnGameStarted -= StartGame;
        GameManager.OnGameOverHandler -= EndGame;
        GameManager.OnGamePause -= PauseGame;
    }

    private void PauseGame() {
        InputManager.Input.OnSwitchInput(BaseInput.InputState.None);
    }

    private void EndGame(ulong obj) {
        InputManager.Input.OnSwitchInput(BaseInput.InputState.None);
    }
    
    private void StartGame() {
        InputManager.Input.OnSwitchInput(BaseInput.InputState.Gameplay);
    }
}
