using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollider : NetworkBehaviour {
    public static Action<Collider> OnCollision;

    public override void OnNetworkSpawn() {
        this.enabled = IsOwner;
        
        if (!IsOwner) return;
        CollisionDetecter.OnCollisionDetected += OnTriggerEnter;
    }

    public override void OnNetworkDespawn() {
        if (!IsOwner) return;
        CollisionDetecter.OnCollisionDetected -= OnTriggerEnter;
    }

    public void OnTriggerEnter(Collider other) {
        Debug.Log("Owner Id: " + OwnerClientId);
        var bullet = other.GetComponent<Bullet>();
        
        if (bullet != null) {
            if(!bullet.IsOwner && IsOwner) {
                OnCollision?.Invoke(other);

                if(IsServer) DeSpawnObjectServerRpc(bullet.NetworkObjectId);
                //Spawner.Instance.DeSpawnObjectServerRpc(bullet.NetworkObjectId);
                Debug.Log("TAK");
            }
            other.gameObject.SetActive(false);
            Debug.Log(" Bullet sender Id:" + bullet.senderId + " OwnerCliend Id:" + OwnerClientId + " Owner: " + IsOwner + " Bullet owner: " + bullet.IsOwner);
        }
        
        Debug.Log("OnTrigger");
    }
    [ServerRpc(RequireOwnership = false)]
    public void DeSpawnObjectServerRpc(ulong id) {
        Debug.Log("DeSpawn Object");
        //Destroy(NetworkManager.Singleton.SpawnManager.SpawnedObjects[id].gameObject);
        NetworkManager.Singleton.SpawnManager.SpawnedObjects[id].gameObject.SetActive(false);
        //DeSpawnObjectClientRpc(id);
    }
    [ClientRpc]
    public void DeSpawnObjectClientRpc(ulong id) {
        NetworkManager.Singleton.SpawnManager.SpawnedObjects[id].gameObject.SetActive(false);
    }
}