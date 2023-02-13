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

        Debug.Log("playerBulletPoller: " + playerBulletPoller);
        var obj = playerBulletPoller.ObjectPoller.GetObject();
        if (obj == null) {
            Debug.Log("NIE ZNALEZIONO OBIEKTU");
        }
        var bulletReference = obj.GetComponent<Bullet>();
        bulletReference.transform.rotation = rotation;
        bulletReference.SetSenderId(senderId);
        bulletReference.SetSpeed(speedBullet);
        bulletReference.MoveTowards(direction);
        bulletReference.gameObject.SetActive(true);
        
        Debug.Log("TAK");
    }
}
