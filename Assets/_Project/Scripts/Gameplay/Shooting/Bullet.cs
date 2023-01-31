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

    public void SetParticleVelocity(Vector3 direction_) {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
        int count = particleSystem.GetParticles(particles);
        for (int i = 0; i < count; i++) {
            particles[i].velocity = direction_ * speed;
        }
        particleSystem.SetParticles(particles, count);
    }
    private void Update() { if (!canMove) return; MoveTo(); }

    protected abstract void MoveTo();
}
