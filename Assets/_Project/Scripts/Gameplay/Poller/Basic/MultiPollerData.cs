using System;
using System.Collections.Generic;
using UnityEngine;

public class MultiPollerData : PollerData
{
    public List<PolledObject> prefabs;
    public override PolledObject GetPolledObject(int index) {
        return prefabs[index];
    }

    public override List<PolledObject> CreatePolledObjects(Transform transform) {
        var tmp_list = new List<PolledObject>();

        int indexer = 0;
        for (int i = 0; i < prefabs.Count; i++) {
            for (int j = 0; j < count; j++) {
                var spawnedObject = Instantiate(GetPolledObject(i), transform);
                spawnedObject.Type = objectPollTypes;
                spawnedObject.ownerId = ownerId;
                spawnedObject.gameObject.SetActive(false);
                spawnedObject.objectId = indexer;
            
                tmp_list.Add(spawnedObject);
                indexer++;
            }
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
