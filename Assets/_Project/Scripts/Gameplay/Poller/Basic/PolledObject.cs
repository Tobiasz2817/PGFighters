
using System;
using UnityEngine;

public abstract class PolledObject : MonoBehaviour
{
    public int objectId;
    [SerializeField] public Rigidbody rb;
    [field: SerializeField] public ObjectPollTypes type { private set; get; }
}
