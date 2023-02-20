using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class BasicGun : Gun
{
    protected override void Fire(ulong senderId, Vector3 bulletDirection) {
        bulletDirection.y = shootPoint.transform.position.y;
        Vector3 direction = (bulletDirection - shootPoint.transform.position).normalized;
        
        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation.z = 0;
        rotation.x = 0;

        var obj = playerBulletPoller.ObjectPoller.GetObject();
        if (obj == null) {
            //Debug.Log("NIE ZNALEZIONO OBIEKTU");

            return;
        }
        var bulletReference = obj.gameObject.GetComponent<Bullet>();
        bulletReference.SetSenderId(senderId);
        bulletReference.SetSpeed(speedBullet);
        bulletReference.transform.rotation = rotation;
        bulletReference.MoveTowards(direction);
    }
}
