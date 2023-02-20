
using System;
using System.Collections.Generic;
using Smooth;
using Unity.Netcode;
using UnityEngine;

public class ObjectPoller : NetworkBehaviour
{
    public List<NetworkObject> listObjects = new List<NetworkObject>();
    public GameObject basicObject;
    public Gun ownerGun;
    public int countObjects = 30;

    public bool MoveWithPlayerGun = false;
    public void InitPoller(GameObject basicObject, int countObjects) {
        this.basicObject = basicObject;
        this.countObjects = countObjects;
    }
    public void CreatePoller(ulong gunRefId) {
        Debug.Log("CREATING POLLER");
        if(basicObject == null) return;
        DestroyPollerObject();
        
        for (int i = 0; i < countObjects; i++) 
            AddObjectServerRpc();
        
        SetRefGunServerRpc(gunRefId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetRefGunServerRpc(ulong clientId) {
        SetRefGunClientRpc(clientId);
    }
    [ClientRpc]
    private void SetRefGunClientRpc(ulong clientId) {
        var gun = NetworkManager.Singleton.SpawnManager.SpawnedObjects[clientId];
        ownerGun = gun.gameObject.GetComponent<PlayerShooting>().GetGunReference();
    }

    public NetworkObject GetObject() {
        foreach (var object_ in listObjects)
            if (!object_.gameObject.activeInHierarchy) {
                ActiveObjectServerRpc(object_.NetworkObjectId,true);
                return object_;
            }
        
        AddObjectServerRpc();
        
        foreach (var object_ in listObjects)
            if (!object_.gameObject.activeInHierarchy) {
                ActiveObjectServerRpc(object_.NetworkObjectId,true); 
                return object_;
            }

        Debug.Log("RETURN NULl");
        
        return null;
    }
    
    public void PollObject(NetworkObject polledObject) {
        if (!listObjects.Contains(polledObject)) {
            Debug.Log(polledObject + "  dont contains object");
            return;
        }
        
        ActiveObjectServerRpc(polledObject.NetworkObjectId, false);
        ResetPositionServerRpc(polledObject.NetworkObjectId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddObjectServerRpc() {
        var object_ =  Spawner.Instance.SpawnNetworkObjectWithOwnership(basicObject, OwnerClientId);
        object_.transform.parent = transform;
        var networkObject = object_.GetComponent<NetworkObject>();
        
        ActiveObjectClientRpc(networkObject.NetworkObjectId, false);
        AddObjectClientRpc(networkObject.NetworkObjectId);
    }
    [ClientRpc]
    private void AddObjectClientRpc(ulong idPolledObject) {
        if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.ContainsKey(idPolledObject)) return;
        var spawnedObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[idPolledObject];
        if (listObjects.Contains(spawnedObject)) return;
        
        listObjects.Add(spawnedObject);
    }
    
    private void DestroyPollerObject() {
        foreach (var object_ in listObjects) 
            Spawner.Instance.DespawnObjectServerRpc(object_.GetComponent<NetworkObject>().NetworkObjectId);
        
        listObjects.Clear();
    }

    
    [ServerRpc(RequireOwnership = false)]
    private void ActiveObjectServerRpc(ulong id,bool isActive) {
        ActiveObjectClientRpc(id,isActive);
    }
    
    [ClientRpc]
    private void ActiveObjectClientRpc(ulong id,bool isActive) {
        if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.ContainsKey(id)) return;
        
        NetworkManager.Singleton.SpawnManager.SpawnedObjects[id].gameObject.SetActive(isActive);
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void ResetPositionServerRpc(ulong id) {
        ResetPositionClientRpc(id);
    }
    
    [ClientRpc]
    private void ResetPositionClientRpc(ulong id) {
        if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.ContainsKey(id)) return;

        var objectPolled = NetworkManager.Singleton.SpawnManager.SpawnedObjects[id].gameObject;
        objectPolled.transform.position = Vector3.zero;
        objectPolled.transform.rotation = Quaternion.identity;
    }
    
    private void Update() {
        if(listObjects == null || ownerGun == null || !MoveWithPlayerGun) return;

        foreach (var networkObject in listObjects) 
            if (!networkObject.gameObject.activeInHierarchy) 
                networkObject.transform.position = ownerGun.GetShootPoint().position;
    }
}