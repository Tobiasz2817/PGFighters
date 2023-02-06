using System;
using Smooth;
using Unity.Netcode;
using UnityEngine;

public abstract class Bullet : NetworkBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    [field: SerializeField] public float Damage { private set; get; }
    public ulong senderId { private set; get; }
    
    protected Vector3 direction;
    protected float speed;
    
    private bool canMove;
    
    public void SetSpeed(float speed_) {
        this.speed = speed_;
    }
    public void SetSenderId(ulong id) {
        this.senderId = id;
    }
    public void MoveTowards(Vector3 direction_) {
        this.direction = direction_;

        canMove = true;
    }
    /*public void SetParticleVelocity(float x,float y,float z) {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
        int count = particleSystem.GetParticles(particles);
        for (int i = 0; i < count; i++) {
            particles[i].velocity = new Vector3(x,y,z) * speed;
        }
        particleSystem.SetParticles(particles, count);
    }*/
    protected abstract void MoveTo();

    protected void DisableSelf() {
        this.gameObject.SetActive(false);
    }
    
    #region Unity function
    private void OnDisable() {
        if (IsOwner) return;
        Spawner.Instance.DespawnObjectServerRpc(NetworkObjectId,true);
    }
    private void Update() { if (!canMove) return; MoveTo(); }
    private void OnTriggerEnter(Collider other) {
        Debug.Log("WTFFF");
        Debug.Log("Other: " + other.name + " Self: " + OwnerClientId + " ObjectId: " + NetworkObjectId + " Is Owner: " + IsOwner);

        DisableSelf();
        if (IsOwner) return;

        if (other.CompareTag("Player") ) {
            var playerReference = other.GetComponent<PlayerReference>();
            if (playerReference != null) {
                playerReference.playerHealth.DealDamage(this);
            }
        }
        
    }
    #endregion
}
