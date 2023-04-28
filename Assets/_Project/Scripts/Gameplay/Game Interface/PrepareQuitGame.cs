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
        if (NetworkManager.Singleton == null) return;
        if (NetworkManager.Singleton.ShutdownInProgress) return;
        
        NetworkManager.Singleton.Shutdown();
        Destroy(NetworkManager.Singleton.gameObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void PreparedGameOverServerRpc(ulong losePlayerId) {
        ulong idLosePlayer = default;
        foreach (var client in NetworkManager.Singleton.ConnectedClients) {
            var player = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(client.Key);
            var tmpPlayer = player.GetComponent<PlayerHealth>();
            if (tmpPlayer.GetHealth() <= 0) {
                idLosePlayer = tmpPlayer.OwnerClientId;
                break;
            }
        }
        PreparingGameOverClientRpc(idLosePlayer);
    }

    [ClientRpc]
    private void PreparingGameOverClientRpc(ulong losePlayerId) {
        Debug.Log("PreparingGameOverClientRpc");
        Debug.Log("Owner Id: " + OwnerClientId + " Lose client Id: " + losePlayerId);
        var LocalPlayer = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
        var endText = LocalPlayer.OwnerClientId != losePlayerId ? "You Win" : "You Lose";
        preparedGameOverInterface.InvokePreparedGame(endText);
    }

    private void ClearNetworkManagerAndDisconnect(ulong clientId) {
        NetworkManager.Singleton.DisconnectClient(clientId);
        NetworkManager.Singleton.Shutdown();
    }
}