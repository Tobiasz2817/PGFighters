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
    }

    public override void OnNetworkDespawn() {
        
    }

    public void DealDamage(Bullet bullet) {
        float damage = bullet.Damage; 
        TakeDamage(damage);
        Debug.Log("I Taking damage: " + damage + " my id: " + OwnerClientId);
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
