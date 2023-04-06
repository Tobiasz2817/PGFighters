using System.Collections;
using UnityEngine;

public abstract class Gun : PolledObject
{
    [SerializeField] public bool animateShoot = true;
    [SerializeField] protected float shootDelay;
    [SerializeField] protected float speedBullet;
    [SerializeField] protected Bullet bullet;
    [SerializeField] protected Transform shootPoint;
    private bool canShoot = true;
    public bool CanShoot { private set => canShoot = value; get => canShoot; }

    public void TryFire(ulong senderId, int bulletId, Vector3 direction) {
        if (canShoot) {
            StartCoroutine(ShootDelay());
            Fire(senderId,bulletId,direction);
        }
    }
    
    private IEnumerator ShootDelay() {
        canShoot = false;
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }

    protected abstract void Fire(ulong senderId,int bulletId, Vector3 direction);

    public Bullet GetGunBullet() {
        return bullet;
    }

    public Transform GetShootPoint() {
        return shootPoint;
    }

    public void ReverseBullets() {
        NetworkPoller.Instance.ReversObjects(ownerId,ObjectPollTypes.GunBullets,bullet.GetType());
    }
}
