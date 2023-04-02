
using System;
using UnityEngine;

public abstract class PolledObject : MonoBehaviour
{
    public int objectId;
    public ulong ownerId;
    [SerializeField] public Rigidbody rb;
    
    [field: SerializeField] public ObjectPollTypes TypeVisibler;
    private ObjectPollTypes type;
    public ObjectPollTypes Type {
        set {
            if (typeWasModifide) return;
            type = value;
            TypeVisibler = value;
            typeWasModifide = true;
        }
        get => type;
    }
    private bool typeWasModifide = false;
}
