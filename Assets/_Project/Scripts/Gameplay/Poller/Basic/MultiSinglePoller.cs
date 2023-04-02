
using System;
using System.Collections.Generic;
using UnityEngine;

public class MultiSinglePoller : PollerData
{
    public List<PolledObject> prefabs;
    public override PolledObject GetPolledObject(int index) {
        return prefabs[index];
    }

    public override List<PolledObject> CreatePolledObjects(Transform transform) {
        var tmp_list = new List<PolledObject>();
    
        for (int i = 0; i < count; i++) {
            var spawnedObject = Instantiate(GetPolledObject(0), transform);
            spawnedObject.Type = objectPollTypes;
            spawnedObject.ownerId = ownerId;
            spawnedObject.gameObject.SetActive(false);
            spawnedObject.objectId = i;
        
            tmp_list.Add(spawnedObject);
        }
        
        return tmp_list;
    }

    public override int GetIndexObjectsByType(Type type) {
        for (int i = 0; i < prefabs.Count; i++) {
            if (prefabs[i].GetType() == type)
                return i;
        }

        return -1;
    }

    public override int GetPrefabsCount() {
        return prefabs.Count;
    }
}
