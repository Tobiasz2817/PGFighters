using System;
using System.Collections;
using Smooth;
using Unity.Netcode;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public abstract class Bullet : NetworkBehaviour
{
    [field: SerializeField] public float Damage { private set; get; }
    public ulong senderId { private set; get; }
    
    protected Vector3 direction;
    protected float speed;
    
    private bool canMove;
    private ObjectPoller objectPoller;
    
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

    protected abstract void MoveTo();

    protected void DisableSelf() {
        if (objectPoller == null) return; 
            
        objectPoller.PollObject(gameObject);
    }
    
    #region Unity function
    private void OnTransformParentChanged() {
        objectPoller = GetComponent<ObjectPoller>();
        Debug.Log("Change Parent");
    }
    private void OnEnable() {
        StartCoroutine(DeActiveSelf(10f));
    }

    private void FixedUpdate() { if (!canMove) return; MoveTo(); }
    private void OnTriggerEnter(Collider other) {
        Debug.Log("WTFFF");
        //Debug.Log("Other: " + other.name + " Self: " + OwnerClientId + " ObjectId: " + NetworkObjectId + " Is Owner: " + IsOwner);

        if (other.CompareTag("Bullet")) return;
        if (other.CompareTag("Player") ) {
            var playerReference = other.GetComponent<PlayerReference>();
            if (IsOwner || !playerReference.playerHealth.IsOwner) return;
            
            if (playerReference != null) {
                playerReference.playerHealth.DealDamage(this);
            }
        }
        DisableSelf();
    }
    #endregion

    private IEnumerator DeActiveSelf(float time) {
        yield return new WaitForSeconds(time);
        DisableSelf();
    }
}
/*public void SetParticleVelocity(float x,float y,float z) {
    ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
    int count = particleSystem.GetParticles(particles);
    for (int i = 0; i < count; i++) {
        particles[i].velocity = new Vector3(x,y,z) * speed;
    }
    particleSystem.SetParticles(particles, count);
}*/