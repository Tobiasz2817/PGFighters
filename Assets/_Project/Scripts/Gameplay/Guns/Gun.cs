
using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [SerializeField] protected float shootDelay;
    [SerializeField] protected float speedBullet;
    [SerializeField] protected Bullet bullet;
    [SerializeField] protected Transform shootPoint;
    private bool canShoot = true;
    public bool CanShoot { private set => canShoot = value; get => canShoot; }
    public int gunId;
    protected PlayerBulletPoller playerBulletPoller;
    private Vector3 direction;

    public void TryFire(ulong senderId, Vector3 direction) {
        if (canShoot) {
            this.direction = direction;
            
            StartCoroutine(ShootDelay());
            Fire(senderId,direction);
        }
    }
    
    private IEnumerator ShootDelay() {
        canShoot = false;
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }

    protected abstract void Fire(ulong senderId, Vector3 direction);

    public Bullet GetGunBullet() {
        return bullet;
    }

    public Transform GetShootPoint() {
        return shootPoint;
    }

    private void OnEnable() {
        var pbp = GetComponentInParent<PlayerBulletPoller>();
        if (pbp != null) {
            playerBulletPoller = pbp;
        }
        else {
            Debug.Log("CANT FIND PLAYERBULLETPOLLER");
        }
    }

    private void OnTransformParentChanged() {
        playerBulletPoller = GetComponentInParent<PlayerBulletPoller>();
        Debug.Log("ON CHANGE  PARENT: " + playerBulletPoller);
    }
}
