using UnityEngine;

public class BasicGun : Gun
{
    protected override void Fire(ulong senderId,int bulletId, Vector3 bulletDirection) {
        bulletDirection.y = GetShootPoint().position.y;
        Vector3 direction = (bulletDirection - GetShootPoint().position).normalized;
        
        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation.z = 0;
        rotation.x = 0;

        var bulletReference = (Bullet)NetworkPoller.Instance.GetObject(senderId,ObjectPollTypes.GunBullets,bulletId);
        bulletReference.transform.position = GetShootPoint().position;
        bulletReference.transform.rotation = rotation;
        bulletReference.SetSpeed(speedBullet);
        bulletReference.MoveTowards(direction);
        bulletReference.SetSenderId(senderId);
        bulletReference.gameObject.SetActive(true);
    }
}
