using System;
using UnityEngine;
using Utilities;

public class GameManager : Singleton<GameManager> 
{
    public Action StartedGame;
    public Action PauseGame;
    public Action<ulong> GameIsOver;

    public static event Action OnGameStarted;
    public static event Action<ulong> OnGameOverHandler;

    private GameState currentState = GameState.Preparing;
    public GameState CurrentState
    {
        private set => currentState = value;
        get => currentState;
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
        OnGameOverHandler?.Invoke(losePlayerId);
    }

    
    private void ChangeGameState(GameState gameState) {
        this.currentState = gameState;
    }

    public enum GameState
    {
        Preparing,
        Started,
        End
    }
}
