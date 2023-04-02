using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrepareGame : NetworkBehaviour
{
    public PreparedGameInterface preparedGameInterface;
    public override void OnNetworkSpawn() {
        if (!IsServer) return;
        
        LoadingSceneManager.Instance.OnClientsConnected += PreparingGame;
    }

    private void PreparingGame(List<ulong> clientscompleted) {
        PreparingGameClientRpc();
    }
    
    [ClientRpc]
    private void PreparingGameClientRpc() {
        preparedGameInterface.InvokePreparedGame();
    }
}
