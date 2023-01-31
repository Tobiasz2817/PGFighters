using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField]
    private float health = 100;

    public static event Action<float> OnHealthChange;

    public override void OnNetworkSpawn() {
       this.enabled = IsOwner;
       
       PlayerCollider.OnCollision += CheckCollision;
    }

    public override void OnNetworkDespawn() {
        PlayerCollider.OnCollision -= CheckCollision;
    }

    private void CheckCollision(Collider obj) {
        if (IsOwner) {
            float damage = obj.GetComponent<Bullet>().Damage; 
            TakeDamage(damage);
            Debug.Log("I Taking damage: " + damage);
        }
    }

    private void ChangeHealth(float newHealth) {
        this.health = newHealth;
        OnHealthChange?.Invoke(health);
        Debug.Log("Current health " + newHealth);
    }

    public void TakeDamage(float damage) {
        ChangeHealth(health - damage);
    }
}
