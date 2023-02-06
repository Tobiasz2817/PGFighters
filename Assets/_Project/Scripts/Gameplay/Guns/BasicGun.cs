using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class BasicGun : Gun
{
    protected override void Fire(ulong senderId, ulong bulletId, Vector3 bulletDirection) {
        bulletDirection.y = shootPoint.transform.position.y;
        Vector3 direction = (bulletDirection - shootPoint.transform.position).normalized;
        
        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation.z = 0;
        rotation.x = 0;

        var bulletReference = NetworkManager.Singleton.SpawnManager.SpawnedObjects[bulletId].GetComponent<Bullet>();
        bulletReference.transform.rotation = rotation;
        bulletReference.SetSenderId(senderId);
        bulletReference.SetSpeed(speedBullet);
        bulletReference.MoveTowards(direction);
        
        Debug.Log("TAK");
    }
}
