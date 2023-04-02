using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

public class Spawner : NetworkBehaviour
{
    public static Spawner Instance;

    private void Awake() {
        Instance = this;
    }
    
    public T SpawnObject<T>(T obj, Vector3 position,Quaternion rotation) where T : Object {
        return Instantiate(obj,position,rotation);
    }
    public T SpawnNetworkObject<T>(T obj, Vector3 position,Quaternion rotation) where T : Object {
        T spawnedObj = Instantiate(obj,position,rotation);
        spawnedObj.GetComponent<NetworkObject>().Spawn();
        return spawnedObj;
    }
    public T SpawnNetworkObjectWithOwnership<T>(T obj, Vector3 position,Quaternion rotation, ulong clientId) where T : Object {
        T spawnedObj = Instantiate(obj,position,rotation);
        spawnedObj.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
        return spawnedObj;
    }
    public T SpawnNetworkObjectWithOwnership<T>(T obj, ulong clientId) where T : Object {
        T spawnedObj = Instantiate(obj);
        spawnedObj.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
        return spawnedObj;
    }
    public T SpawnNetworkObject<T>(T obj, Vector3 position,Quaternion rotation, Transform parent) where T : Object {
        T spawnedObj = Instantiate(obj,position,rotation,parent);
        spawnedObj.GetComponent<NetworkObject>().Spawn();
        return spawnedObj;
    }

    public T SpawnPlayer<T>(T obj,Vector3 position,Quaternion rotation, ulong id) where T : Object {
        T spawnedObj = Instantiate(obj,position,rotation);
        spawnedObj.GetComponent<NetworkObject>().SpawnAsPlayerObject(id);
        return spawnedObj;
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void DespawnObjectServerRpc(ulong objectId, bool destroy = true) {
        if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.ContainsKey(objectId)) return;
        var objToDespawn = NetworkManager.Singleton.SpawnManager.SpawnedObjects[objectId];
        objToDespawn.Despawn(destroy);
    }
    [ServerRpc(RequireOwnership = false)]
    public void DespawnPlayerServerRpc(ulong clientId, bool destroy = true) {
        var player = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientId);
        if (!player) return;
        player.Despawn(destroy);
    }
    
    #region Network
    
    [ServerRpc(RequireOwnership = false)]
    public void DeSpawnObjectServerRpc(ulong id) {
        if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.ContainsKey(id)) {
            Debug.LogError("Cant despawn object because there no exist in dictionary");
            return;
        }
        Debug.Log("DeSpawn Object");
        NetworkManager.Singleton.SpawnManager.SpawnedObjects[id].Despawn();
    }


    [ServerRpc(RequireOwnership = false)]
    public void DeSpawnPlayerObjectServerRpc(ulong clientId) {
        DeSpawnPlayerObject(clientId);
    }
    
    private void DeSpawnPlayerObject(ulong clientId) {
        var player = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientId);
        
        if (player == null) {
            Debug.LogError("Cant despawn player because there no exist in dictionary");
            return;
        }
        
        player.Despawn();
    }

    #endregion
}