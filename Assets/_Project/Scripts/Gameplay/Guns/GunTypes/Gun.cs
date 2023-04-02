using System.Collections;
using UnityEngine;

public abstract class Gun : PolledObject
{
    [SerializeField] protected float shootDelay;
    [SerializeField] protected float speedBullet;
    [SerializeField] protected Bullet bullet;
    [SerializeField] protected Transform shootPoint;
    private bool canShoot = true;
    public bool CanShoot { private set => canShoot = value; get => canShoot; }

    public void TryFire(ulong senderId, Vector3 direction) {
        if (canShoot) {
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

    public void ReverseBullets() {
        /*var index = NetworkPoller.Instance.GetIndexPrefab(ownerId, ObjectPollTypes.GunBullets, bullet.GetType());
        if (index == -1) {
            Debug.Log("Error: " + index + " Can't reversing object on this type because didn't exist");
        }*/
        NetworkPoller.Instance.ReversObjects(ownerId,ObjectPollTypes.GunBullets,bullet.GetType());
    } 
}
