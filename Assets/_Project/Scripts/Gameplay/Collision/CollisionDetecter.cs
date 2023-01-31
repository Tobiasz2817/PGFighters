using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetecter : MonoBehaviour
{
    public static event Action<Collider> OnCollisionDetected;
    
    public void OnTriggerEnter(Collider other) {
        Debug.Log("Detect collision: " + other.name);
        OnCollisionDetected?.Invoke(other);
    }
}
