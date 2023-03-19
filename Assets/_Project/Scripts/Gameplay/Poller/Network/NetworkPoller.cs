using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Utilities;

[RequireComponent(typeof(PollerData))]
public class NetworkPoller : SingletonNetwork<NetworkPoller>
{
    [SerializeField] 
    private Dictionary<ulong,Dictionary<ObjectPollTypes,PollerPlaceHolder>> objectsToPoll = new Dictionary<ulong, Dictionary<ObjectPollTypes, PollerPlaceHolder>>();

    public override void Awake() {
        base.Awake();
        LoadingSceneManager.Instance.OnClientsConnected += InitPoller;
        NetworkManager.Singleton.OnClientDisconnectCallback += ClientDisconnect;
    }

    #region Polling Operations

    #region Get Without Polling
    
    public T GetObject<T> (ulong ownerId, ObjectPollTypes typeObject,int index) where T : PolledObject {
        return (T)objectsToPoll[ownerId][typeObject].GetObject(index);
    }
    public T GetObject<T> (ulong ownerId, ObjectPollTypes typeObject,int index, Vector3 position, Quaternion rotation, Vector3 scale) where T : PolledObject {
        var getObj = objectsToPoll[ownerId][typeObject].GetObject(index);
        getObj.transform.position = position;
        getObj.transform.rotation = rotation;
        getObj.transform.localScale = scale;
        
        return (T)getObj;
    }
    public T GetObject<T> (ulong ownerId, ObjectPollTypes typeObject,int index, Vector3 position, Quaternion rotation) where T : PolledObject {
        var getObj = objectsToPoll[ownerId][typeObject].GetObject(index);
        getObj.transform.position = position;
        getObj.transform.rotation = rotation;
        
        return (T)getObj;
    }
    

    #endregion

    #region Get With Polling
    
    public T GetObject<T> (ulong ownerId, ObjectPollTypes typeObject,bool state) where T : PolledObject {
        var getObj = objectsToPoll[ownerId][typeObject].GetUnActiveObject();
        StateActiveObjectServerRpc(ownerId,typeObject,getObj.objectId,state);
        
        return (T)getObj;
    }
    public T GetObject<T> (ulong ownerId, ObjectPollTypes typeObject) where T : PolledObject {
        return (T)objectsToPoll[ownerId][typeObject].GetUnActiveObject();
    }
    public T GetObject<T> (ulong ownerId, ObjectPollTypes typeObject, Vector3 position, Quaternion rotation) where T : PolledObject {
        var getObj = objectsToPoll[ownerId][typeObject].GetUnActiveObject();
        PlaceObjectOnPositionServerRpc(ownerId,typeObject,getObj.objectId,position,rotation);
        
        return (T)getObj;
    }
    public T GetObject<T> (ulong ownerId, ObjectPollTypes typeObject, Vector3 position, Quaternion rotation, Vector3 scale) where T : PolledObject {
        var getObj = objectsToPoll[ownerId][typeObject].GetUnActiveObject();
        PlaceObjectOnPositionServerRpc(ownerId,typeObject,getObj.objectId,position,rotation,scale);
        
        return (T)getObj;
    }
    public PolledObject GetObject (ulong ownerId, ObjectPollTypes typeObject) {
        return objectsToPoll[ownerId][typeObject].GetUnActiveObject();
    }

    #endregion
    

    
    [ServerRpc(RequireOwnership = false)]
    public void PollObjectServerRpc(ulong ownerId, ObjectPollTypes typeObject, int id) {
        PollObjectClientRpc(ownerId, typeObject, id);
    }
    [ClientRpc]
    public void PollObjectClientRpc(ulong ownerId, ObjectPollTypes typeObject, int id) {
        PollObject(ownerId, typeObject, id);
    }
    public void PollObject(ulong ownerId, ObjectPollTypes typeObject, int id) {
        objectsToPoll[ownerId][typeObject].PollActiveObject(id);
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void PlaceObjectOnPositionServerRpc(ulong ownerId, ObjectPollTypes typeObject,int index, Vector3 position, Quaternion rotation, Vector3 scale) {
        PlaceObjectOnPositionClientRpc(ownerId, typeObject, index,position,rotation,scale);
    }
    [ClientRpc]
    public void PlaceObjectOnPositionClientRpc(ulong ownerId, ObjectPollTypes typeObject,int index , Vector3 position, Quaternion rotation, Vector3 scale) {
        PlaceObject(ownerId, typeObject, index,position,rotation,scale);
    }
    public void PlaceObject(ulong ownerId, ObjectPollTypes typeObject,int index, Vector3 position, Quaternion rotation, Vector3 scale) {
        objectsToPoll[ownerId][typeObject].GetObject(index).transform.position = position;
        objectsToPoll[ownerId][typeObject].GetObject(index).transform.rotation = rotation;
        objectsToPoll[ownerId][typeObject].GetObject(index).transform.localScale = scale;
    }
    
    
    
    [ServerRpc(RequireOwnership = false)]
    public void PlaceObjectOnPositionServerRpc(ulong ownerId, ObjectPollTypes typeObject,int index, Vector3 position, Quaternion rotation) {
        PlaceObjectOnPositionClientRpc(ownerId, typeObject, index,position,rotation);
    }
    [ClientRpc]
    public void PlaceObjectOnPositionClientRpc(ulong ownerId, ObjectPollTypes typeObject,int index , Vector3 position, Quaternion rotation) {
        PlaceObject(ownerId, typeObject, index,position,rotation);
    }

    public void PlaceObject(ulong ownerId, ObjectPollTypes typeObject,int index, Vector3 position, Quaternion rotation) {
        objectsToPoll[ownerId][typeObject].GetObject(index).transform.position = position;
        objectsToPoll[ownerId][typeObject].GetObject(index).transform.rotation = rotation;
    }

    #endregion

    #region Object state visibly

    [ServerRpc(RequireOwnership = false)]
    private void StateActiveObjectServerRpc(ulong ownerId, ObjectPollTypes typeObject, int id, bool state) {
        StateActiveObjectClientRpc(ownerId, typeObject, id, state);
    }
    [ClientRpc]
    private void StateActiveObjectClientRpc(ulong ownerId, ObjectPollTypes typeObject, int id, bool state) {
        objectsToPoll[ownerId][typeObject].GetObject(id).gameObject.SetActive(state);
    }

    public PolledObject GetObject (ulong ownerId, ObjectPollTypes typeObject,bool state) {
        var polledObject = objectsToPoll[ownerId][typeObject].GetUnActiveObject();
        StateActiveObjectServerRpc(ownerId,typeObject,polledObject.objectId,state);
        return polledObject;
    }
    
    public PolledObject[] GetObjects (ulong ownerId, ObjectPollTypes typeObject,int objectsCount) {
        var polledObjects = objectsToPoll[ownerId][typeObject].GetUnActiveObjects(objectsCount);

        return polledObjects;
    }
    
    public int[] GetIndexes (ulong ownerId, ObjectPollTypes typeObject,int objectsCount) {
        var polledObjects = objectsToPoll[ownerId][typeObject].GetUnActiveIndexes(objectsCount);

        return polledObjects;
    }
    
    #endregion
    

    
    
    #region Init Poller
    private void InitPoller(List<ulong> obj) {
        if (!IsServer) return;
        InitPollerClientRpc(obj.ToArray(),PlayersReferences(obj.Count));
    }

    // Can Invoke Only Server
    private NetworkObjectReference[] PlayersReferences(int connectedPlayers) {
        var references = new NetworkObjectReference[connectedPlayers];

        int x = 0;
        foreach (var player in NetworkManager.Singleton.ConnectedClients.Values) {
            references[x] = player.PlayerObject;
            x++;
        }

        return references;
    }
    
    [ClientRpc]
    private void InitPollerClientRpc(ulong[] clients, NetworkObjectReference[] references) {
        for (int i = 0; i < clients.Length; i++) {
            GameObject placeHolder = new GameObject(clients[i].ToString());
            placeHolder.transform.parent = transform;
            if (references[i].TryGet(out NetworkObject networkObject)) {
                var gameObjectsList = CreatePlayersObject(placeHolder.transform,networkObject.transform);
            
                objectsToPoll.Add(clients[i],gameObjectsList);
            }
        }
    }

    private Dictionary<ObjectPollTypes,PollerPlaceHolder> CreatePlayersObject(Transform folderParent,Transform transform) {
       
        Dictionary<ObjectPollTypes,PollerPlaceHolder> newObjects = new Dictionary<ObjectPollTypes,PollerPlaceHolder>();
        foreach (var component in GetComponents<PollerData>()) {
            if(!component.isNetwork)
            {
                GameObject placeHolder = new GameObject(component.prefab.type.ToString());
                placeHolder.transform.parent = folderParent;
                var placeHolderTmp = placeHolder.AddComponent<PollerPlaceHolder>();
                placeHolderTmp.InitPoller(component.count,component.prefab);
                placeHolderTmp.InitTransform(transform);
                newObjects.Add(component.prefab.type,placeHolderTmp);
            }
        }

        return newObjects;
    }
    
    #endregion

    #region Destroy Objects
    
    private void ClientDisconnect(ulong id) {
        if (IsServer) {
            DestroyKeyClientRpc(id);
        }
    }

    [ClientRpc]
    private void DestroyKeyClientRpc(ulong id) {
        if (objectsToPoll.ContainsKey(id)) {
            foreach (var objects in objectsToPoll[id]) {
                Destroy(objects.Value.gameObject);
            }

            objectsToPoll.Remove(id);
        }
    }

    #endregion
}

