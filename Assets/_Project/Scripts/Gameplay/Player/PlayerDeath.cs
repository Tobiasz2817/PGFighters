using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerDeath : NetworkBehaviour {
    public override void OnNetworkSpawn() {
        this.enabled = IsOwner;
        PlayerHealth.OnHealthChange += CheckHealth;
    }

    public override void OnNetworkDespawn() {
        PlayerHealth.OnHealthChange -= CheckHealth;
    }

    private void CheckHealth(float health) {
        if (health <= 0) {
            Debug.Log("I die");
            GameManager.Instance.GameIsOver?.Invoke(OwnerClientId);
            //Spawner.Instance.DeSpawnPlayerObjectServerRpc(OwnerClientId);
            //Destroy(this);
            DisablePlayerServerRpc(OwnerClientId);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DisablePlayerServerRpc(ulong id) {
        DisablePlayerClientRpc(id);
    }
    [ClientRpc]
    private void DisablePlayerClientRpc(ulong id) {
        Debug.Log(NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(id).gameObject.name);
        NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(id).gameObject.SetActive(false);
    }
}