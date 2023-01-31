using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PrepareQuitGame : NetworkBehaviour {
    public PrepareQuitGameInterface preparedGameOverInterface;

    public override void OnNetworkSpawn() {
        GameManager.OnGameOverHandler += PreparedGameOverServerRpc;
    }

    public override void OnNetworkDespawn() {
        GameManager.OnGameOverHandler -= PreparedGameOverServerRpc;
    }

    
    [ServerRpc(RequireOwnership = false)]
    private void PreparedGameOverServerRpc(ulong losePlayerId) {
        Debug.Log("invokee");
        PreparingGameOverClientRpc(losePlayerId);
    }

    [ClientRpc]
    private void PreparingGameOverClientRpc(ulong losePlayerId) {
        if (!IsOwner) return;
        Debug.Log("Client Preparing game over");
        Debug.Log("Owner Id: " + OwnerClientId + " Lose client Id: " + losePlayerId);
        //ClearNetworkManagerAndDisconnect(OwnerClientId);
        preparedGameOverInterface.InvokePreparedGame(OwnerClientId != losePlayerId ? "You Win" : "You Lose");
    }

    private void ClearNetworkManagerAndDisconnect(ulong clientId) {
        NetworkManager.Singleton.DisconnectClient(clientId);
        NetworkManager.Singleton.Shutdown();
    }
}