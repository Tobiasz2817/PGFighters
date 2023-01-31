using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    private GameState gameState = GameState.Preparing;

    public Action StartedGame;
    public Action PauseGame;
    public Action<ulong> GameIsOver;

    public static event Action OnGameStarted;
    public static event Action<ulong> OnGameOverHandler;

    public void Awake() {
        Instance = this;
        Debug.Log("Game Manager Awake");
    }

    private void OnEnable() {
        StartedGame += StartGame;
        GameIsOver += GameOver;
    }

    private void OnDisable() {
        StartedGame -= StartGame;
        GameIsOver -= GameOver;
    }

    private void StartGame() {
        ChangeGameState(GameState.Started);
        
        OnGameStarted?.Invoke();
    }
    private void GameOver(ulong losePlayerId) {
        ChangeGameState(GameState.End);
        Debug.Log("Game Over");
        OnGameOverHandler?.Invoke(losePlayerId);
    }
    private void ChangeGameState(GameState gameState) {
        this.gameState = gameState;
    }
    public GameState GetGameState() {
        return gameState;
    }
    
    public enum GameState
    {
        Preparing,
        Started,
        End
    }
}
