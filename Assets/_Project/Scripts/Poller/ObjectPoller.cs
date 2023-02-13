
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ObjectPoller : NetworkBehaviour
{
    public List<GameObject> listObjects = new List<GameObject>();
    public GameObject basicObject;
    public int countObjects = 30;
    public void InitPoller(GameObject basicObject, int countObjects) {
        this.basicObject = basicObject;
        this.countObjects = countObjects;
    }
    public void CreatePoller() {
        Debug.Log("CREATING POLLER");
        if(basicObject == null) return;
        DestroyPollerObject();
        
        for (int i = 0; i < countObjects; i++) 
            AddObjectServerRpc();
        
    }

    public GameObject GetObject() {
        foreach (var object_ in listObjects) 
            if (!object_.gameObject.activeInHierarchy)
                return object_;
        
        AddObjectServerRpc();
        
        foreach (var object_ in listObjects) 
            if (!object_.gameObject.activeInHierarchy)
                return object_;

        return null;
    }

    public void PollObject(GameObject polledObject) {
        if (!listObjects.Contains(polledObject)) return;
        
        polledObject.gameObject.SetActive(false);
        polledObject.transform.position = Vector3.zero;
        polledObject.transform.rotation = Quaternion.identity;
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddObjectServerRpc() {
        var object_ =  Spawner.Instance.SpawnNetworkObjectWithOwnership(basicObject, OwnerClientId);
        object_.transform.parent = transform;
        object_.gameObject.SetActive(false);
        listObjects.Add(object_);
    }
    
    private void DestroyPollerObject() {
        foreach (var object_ in listObjects) 
            Spawner.Instance.DespawnObjectServerRpc(object_.GetComponent<NetworkObject>().NetworkObjectId);
        
        listObjects.Clear();
    }
}
