
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
    private Vector3 direction;
    
    public void TryFire(ulong senderId, ulong bulletId, Vector3 direction) {
        if (canShoot) {
            this.direction = direction;
            
            StartCoroutine(ShootDelay());
            Fire(senderId,bulletId,direction);
        }
    }
    
    private IEnumerator ShootDelay() {
        canShoot = false;
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }

    protected abstract void Fire(ulong senderId, ulong bulletId, Vector3 direction);

    public Bullet GetGunBullet() {
        return bullet;
    }

    public Transform GetShootPoint() {
        return shootPoint;
    }
}
