using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Unity.Netcode;
using UnityEngine;
using Utilities;

[RequireComponent(typeof(PollerData))]
public class NetworkPoller : SingletonNetwork<NetworkPoller>
{
    [SerializeField] 
    private Dictionary<ulong,Dictionary<ObjectPollTypes,PollerPlaceHolder>> objectsToPoll = new Dictionary<ulong, Dictionary<ObjectPollTypes, PollerPlaceHolder>>();

    public event Action OnPollerInstances; 

    public override void Awake() {
        base.Awake();
        PlayerSpawn.OnPlayersSpawned += InitPoller;
        NetworkManager.Singleton.OnClientDisconnectCallback += ClientDisconnect;
    }
    
    public IEnumerable<ulong> GetKeys() {
        foreach (var key in objectsToPoll) 
            yield return key.Key;
    }

    #region Callback PolledObjects
    
    public PolledObject GetObject (ulong ownerId, ObjectPollTypes typeObject,bool state) {
        var polledObject = objectsToPoll[ownerId][typeObject].GetUnActiveObject();
        StateActiveObjectServerRpc(ownerId,typeObject,polledObject.objectId,state);
        return polledObject;
    }

    public PolledObject[] GetObjects (ulong ownerId, ObjectPollTypes typeObject,int objectsCount) {
        var polledObjects = objectsToPoll[ownerId][typeObject].GetUnActiveObjects(objectsCount);

        return polledObjects;
    }
    public PolledObject[] GetObjects (ulong ownerId, ObjectPollTypes typeObject,Type type, int countObjects) {
        return objectsToPoll[ownerId][typeObject].GetUnActiveObjects(type,countObjects);
    }
    public List<PolledObject> GetListObjects (ulong ownerId, ObjectPollTypes typeObject,Type type, int countObjects) {
        return objectsToPoll[ownerId][typeObject].GetUnActiveListObjects(type,countObjects);
    }
    public PolledObject GetObject (ulong ownerId, ObjectPollTypes typeObject,int index) {
        return objectsToPoll[ownerId][typeObject].GetObject(index);
    }
    public PolledObject GetActiveObject (ulong ownerId, ObjectPollTypes typeObject,int index) {
        return objectsToPoll[ownerId][typeObject].GetActiveObject(index);
    }
    public PolledObject GetObject (ulong ownerId, ObjectPollTypes typeObject) {
        return objectsToPoll[ownerId][typeObject].GetUnActiveObject();
    }

    #endregion

    #region Callback Genercis T
    
    public T GetObject<T> (ulong ownerId, ObjectPollTypes typeObject,int index) where T : PolledObject {
        return (T)objectsToPoll[ownerId][typeObject].GetObject(index);
    }
    
    public T GetObjectByGenericType<T> (ulong ownerId, ObjectPollTypes typeObject) where T : PolledObject {
        return (T)objectsToPoll[ownerId][typeObject].GetUnActiveObject(typeof(T));
    }
    public T[] GetObjectsByGenericType<T> (ulong ownerId, ObjectPollTypes typeObject,int count) where T : PolledObject {
        return (T[])objectsToPoll[ownerId][typeObject].GetUnActiveObjects(typeof(T),count);
    }
    
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

    #endregion

    #region Callback Index

    public int GetIndexObject<T> (ulong ownerId, ObjectPollTypes typeObject) where T : PolledObject {
        return objectsToPoll[ownerId][typeObject].GetIndexObject(typeof(T));
    }
    
    public int GetIndexPrefab(ulong ownerId, ObjectPollTypes typeObject, Type type) {
        return objectsToPoll[ownerId][typeObject].GetIndexPrefab(type);
    }
    
    public int GetIndexObject (ulong ownerId, ObjectPollTypes typeObject, Type type) {
        return objectsToPoll[ownerId][typeObject].GetIndexObject(type);
    }
    
    public List<int> GetObjectsIndexesDifferentTypes(ulong ownerId, ObjectPollTypes typeObject,int countPerType) {
        return objectsToPoll[ownerId][typeObject].GetObjectsIndexesDifferentTypes(countPerType);
    }
    
        
    public int[] GetIndexes(ulong ownerId, ObjectPollTypes typeObject,int objectsCount) {
        var polledObjects = objectsToPoll[ownerId][typeObject].GetUnActiveIndexes(objectsCount);

        return polledObjects;
    }
    
    public int GetIndex(ulong ownerId, ObjectPollTypes typeObject) {
        var polledObjects = objectsToPoll[ownerId][typeObject].GetUnActiveIndex();

        return polledObjects;
    }
    
    #endregion
    

    #region Network Operations 

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
    
    public void PollObject(PolledObject polledObject) {
        objectsToPoll[polledObject.ownerId][polledObject.Type].PollActiveObject(polledObject.objectId);
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
    
    [ServerRpc(RequireOwnership = false)]
    private void StateActiveObjectServerRpc(ulong ownerId, ObjectPollTypes typeObject, int id, bool state) {
        StateActiveObjectClientRpc(ownerId, typeObject, id, state);
    }
    [ClientRpc]
    private void StateActiveObjectClientRpc(ulong ownerId, ObjectPollTypes typeObject, int id, bool state) {
        objectsToPoll[ownerId][typeObject].GetObject(id).gameObject.SetActive(state);
    }

    #endregion
    

    
    
    #region Init Poller
    
    // TODO:: Feature make to player by connection to game spawn self Polled Objects
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
                var gameObjectsList = CreatePlayersObject(placeHolder.transform,networkObject);
            
                Debug.Log("Counting times");
                objectsToPoll.Add(clients[i],gameObjectsList);
            }
        }
        OnPollerInstances?.Invoke();
    }

    private Dictionary<ObjectPollTypes,PollerPlaceHolder> CreatePlayersObject(Transform folderParent,NetworkObject networkObject) {
       
        Dictionary<ObjectPollTypes,PollerPlaceHolder> newObjects = new Dictionary<ObjectPollTypes,PollerPlaceHolder>();
        foreach (var component in GetComponents<PollerData>()) {
            component.ownerId = networkObject.OwnerClientId;
            GameObject placeHolder = new GameObject(component.objectPollTypes.ToString());
            placeHolder.transform.parent = folderParent;
            var placeHolderTmp = placeHolder.AddComponent<PollerPlaceHolder>();
            placeHolderTmp.InitPoller(component);
            placeHolderTmp.InitTransform(networkObject.transform);
            newObjects.Add(component.objectPollTypes,placeHolderTmp);
        }

        return newObjects;
    }
    
    #endregion

    #region Add / Remove / Revers Operations
    
    [ServerRpc(RequireOwnership = false)]
    public void ReversObjectsServerRpc(ulong playerId,ObjectPollTypes pollTypes,int index) {
        ReversObjectsClientRpc(playerId, pollTypes,index);
    }
    [ClientRpc]
    private void ReversObjectsClientRpc(ulong playerId,ObjectPollTypes pollTypes,int index) {
       ReversObjects(playerId,pollTypes,index);
    }
    public void ReversObjects(ulong playerId,ObjectPollTypes pollTypes,int index) {
        if (objectsToPoll.ContainsKey(playerId)) {
            if (objectsToPoll[playerId].ContainsKey(pollTypes)) {
                objectsToPoll[playerId][pollTypes].ReverseOnNewObject(index);
            }
        }
    }
    
    public void ReversObjects(ulong playerId,ObjectPollTypes pollTypes,Type type) {
        Debug.Log(playerId + " " + pollTypes + " type: " + type);
        if (objectsToPoll.ContainsKey(playerId)) {
            if (objectsToPoll[playerId].ContainsKey(pollTypes)) {
                objectsToPoll[playerId][pollTypes].ReverseOnNewObject(type);
                Debug.Log("Reversing");
            }
        }
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


/* Archive
 
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

*/