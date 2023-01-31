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

        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += PreparingGame;
    }

    private void PreparingGame(string scenename, LoadSceneMode loadscenemode, List<ulong> clientscompleted, List<ulong> clientstimedout) {
        PreparingGameClientRpc();
    }
    
    [ClientRpc]
    private void PreparingGameClientRpc() {
        preparedGameInterface.InvokePreparedGame();
    }
}
