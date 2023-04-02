
using System;
using System.Collections.Generic;
using UnityEngine;

public class SinglePollerData : PollerData
{
    public PolledObject prefab;
    
    public override PolledObject GetPolledObject(int index) {
        return prefab;
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
        return 0;
    }

    public override int GetPrefabsCount() {
        return 1;
    }
}
