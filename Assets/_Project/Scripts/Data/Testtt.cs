using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Testtt : MonoBehaviour
{
    /*[SerializeField]
    private PlayerInput _playerInput;

    public static string PLAYER_DATA_FILE = "PlayerFile"; 

    public static Action OnLoad;
    public static Action OnSave;

    private void Awake()
    {
        _playerInput.actions["Load"].performed += Load;
        _playerInput.actions["Save"].performed += Save;

        OnLoad += LoadData;
        OnSave += SaveData;
    }

    private void Save(InputAction.CallbackContext obj)
    {
        OnLoad?.Invoke();
    }

    private void Load(InputAction.CallbackContext obj)
    {
        OnSave?.Invoke();
    }

    public void LoadData()
    {
        var playerData = SaveManager.LoadDates<GameObject>(PLAYER_DATA_FILE);
        foreach (var player in playerData)
        {
            Debug.Log(player.name);
            var testsplayer = player.GetComponent<PlayerInput>();
            Debug.Log(testsplayer.playerIndex);
            Debug.Log(testsplayer.defaultActionMap);
        }

        Debug.Log("Load Data");
    }
    public void SaveData()
    {
        SaveManager.SaveDates(new [] {gameObject},PLAYER_DATA_FILE);
        
        Debug.Log("Save Data");
    }*/
}
[Serializable]
public struct PlayerData
{
    public int damage;
    public string name;
}
