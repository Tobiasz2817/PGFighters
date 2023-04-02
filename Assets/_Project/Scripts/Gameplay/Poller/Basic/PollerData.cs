using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PollerData : MonoBehaviour
{
    [HideInInspector]
    public ulong ownerId;
    public bool syncWithOwner = true;
    public int count = 1;
    public ObjectPollTypes objectPollTypes;
    public abstract PolledObject GetPolledObject(int index);
    public abstract List<PolledObject> CreatePolledObjects(Transform transform);
    public abstract int GetIndexObjectsByType(Type type);
    public abstract int GetPrefabsCount();
}
