using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerDeath : NetworkBehaviour {
    public override void OnNetworkSpawn() {
        this.enabled = IsOwner;
        if (!IsOwner) return;
        PlayerHealth.OnHealthChange += CheckHealth;
    }

    public override void OnNetworkDespawn() {
        if (!IsOwner) return;
        PlayerHealth.OnHealthChange -= CheckHealth;
    }

    private void CheckHealth(float health) {
        if (health <= 0) {
            Debug.Log("I die");
            GameManager.Instance.GameIsOver?.Invoke(OwnerClientId);
            
            DisablePlayerServerRpc(OwnerClientId);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DisablePlayerServerRpc(ulong id) {
        var player = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(id);
        if (player == null) return;
        DisableActivePlayerClientRpc();
    }
    [ClientRpc]
    private void DisableActivePlayerClientRpc() {
        gameObject.SetActive(false);
    }
}