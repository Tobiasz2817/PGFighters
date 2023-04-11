using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : Gun
{
    [SerializeField] private float rangeLaser = 10f;

    private Vector3 tmpDirection;
    private Vector3 calcTmpDirection;
    protected override void Fire(ulong senderId,int bulletId, Vector3 direction) {
        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation.z = 0;
        rotation.x = 0;

        direction.y = GetShootPoint().position.y;
        Vector3 calculateDirection = (direction - GetShootPoint().position).normalized;

        tmpDirection = direction;
        calcTmpDirection = calculateDirection;
        
        var bulletReference = (LaserBullet)NetworkPoller.Instance.GetObject(senderId, ObjectPollTypes.GunBullets, bulletId);
        bulletReference.transform.position = GetShootPoint().position;
        bulletReference.transform.rotation = rotation;
        bulletReference.SetSenderId(senderId);
        bulletReference.SetSpeed(speedBullet);
        bulletReference.MoveTowards(calculateDirection);
        
        var rayDirection = new Ray();
        rayDirection.origin = GetShootPoint().position;
        rayDirection.direction = calculateDirection;
        bulletReference.SetStopDistance(rayDirection.direction * rangeLaser);

        bulletReference.gameObject.SetActive(true);
    }
    
#if UNITY_EDITOR
    public void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        var rayDirection = new Ray(GetShootPoint().position,tmpDirection);
        Gizmos.DrawRay(rayDirection.origin, rayDirection.direction * rangeLaser);
        Gizmos.color = Color.red;
        var rayDirection2 = new Ray(GetShootPoint().position,calcTmpDirection);
        Gizmos.DrawRay(rayDirection2.origin, rayDirection2.direction * rangeLaser);
    }
    
#endif    
    
}
