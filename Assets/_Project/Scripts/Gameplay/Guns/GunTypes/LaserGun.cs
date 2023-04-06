using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : Gun
{
    [SerializeField] private float rangeLaser = 10f;
    protected override void Fire(ulong senderId,int bulletId, Vector3 direction) {
        Quaternion rotation = Quaternion.LookRotation(GetShootPoint().transform.forward);
        rotation.z = 0;
        rotation.x = 0;

        var rayDirection = new Ray();
        rayDirection.origin = GetShootPoint().transform.position;
        rayDirection.direction = direction * rangeLaser;
        
        var bulletReference = (LaserBullet)NetworkPoller.Instance.GetObject(ownerId, Type, bulletId);
        bulletReference.gameObject.SetActive(true);
        bulletReference.SetStopDistance(rayDirection.direction);
        bulletReference.SetSenderId(senderId);
        bulletReference.SetSpeed(speedBullet);
        bulletReference.transform.rotation = rotation;
        bulletReference.MoveTowards(direction);
    }
    
#if UNITY_EDITOR
    public void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        var direction = -transform.up * rangeLaser;
        var rayDirection = new Ray(GetShootPoint().transform.position,direction);
        Gizmos.DrawRay(rayDirection.origin, direction);
    }
    
#endif    
    
}
