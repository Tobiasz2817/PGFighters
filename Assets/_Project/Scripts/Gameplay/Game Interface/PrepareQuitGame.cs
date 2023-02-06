using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrepareQuitGame : NetworkBehaviour {
    public PrepareQuitGameInterface preparedGameOverInterface;

    public override void OnNetworkSpawn() {
        GameManager.OnGameOverHandler += PreparedGameOverServerRpc;
        PrepareQuitGameInterface.OnGameOverInterfaceFinish += DestroyEverthing;
    }

    public override void OnNetworkDespawn() {
        GameManager.OnGameOverHandler -= PreparedGameOverServerRpc;
        PrepareQuitGameInterface.OnGameOverInterfaceFinish -= DestroyEverthing;
        SceneManager.LoadScene("UI", LoadSceneMode.Single);
    }

    private void DestroyEverthing() {
        ShutdownGameServerRpc();
        Debug.Log("Destroy");
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShutdownGameServerRpc() {
        if (Unity.Netcode.NetworkManager.Singleton == null) return;
        if (NetworkManager.Singleton.ShutdownInProgress) return;
        
        NetworkManager.Singleton.Shutdown();
        Destroy(NetworkManager.Singleton.gameObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void PreparedGameOverServerRpc(ulong losePlayerId) {
        PreparingGameOverClientRpc(losePlayerId);
    }

    [ClientRpc]
    private void PreparingGameOverClientRpc(ulong losePlayerId) {
        Debug.Log("PreparingGameOverClientRpc");
        Debug.Log("Owner Id: " + OwnerClientId + " Lose client Id: " + losePlayerId);

        var endText = IsOwner ? "You Win" : "You Lose";
        preparedGameOverInterface.InvokePreparedGame(endText);
    }

    private void ClearNetworkManagerAndDisconnect(ulong clientId) {
        NetworkManager.Singleton.DisconnectClient(clientId);
        NetworkManager.Singleton.Shutdown();
    }
}