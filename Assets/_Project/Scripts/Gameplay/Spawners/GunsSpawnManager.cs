using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class GunsSpawnManager : NetworkBehaviour
{
    public List<Transform> possiblySpawnedPoints = new List<Transform>();
    public Quaternion gunRotation;
    
    private Dictionary<int, Transform> gunsPositions = new Dictionary<int, Transform>();

    private int countPerType = 2;
    private RandomTypes randomTypes;
    private void Awake() {
        randomTypes = new RandomTypes(typeof(BasicGun), typeof(LaserGun));
    }

    public override void OnNetworkSpawn() {
        if (!IsServer) return;
        NetworkPoller.Instance.OnPollerInstances += EntrySpawnGuns;
        GunPickUp.OnPlayerGunPickUp += StartRespawnGun;
    }
    
    public override void OnNetworkDespawn() {
        if (NetworkPoller.Instance == null) return;
        NetworkPoller.Instance.OnPollerInstances -= EntrySpawnGuns;
        GunPickUp.OnPlayerGunPickUp -= StartRespawnGun;
    }
    
    private void StartRespawnGun(ulong ownerId, int index) {
        /*gun.transform.position = gunPosition.position + Vector3.up;
        gun.transform.rotation = gunRotation;
        gun.gameObject.SetActive(true);

        var gunBarrier = gun.gameObject.GetComponentInChildren<GunBarrierHandler>();
        if (gunBarrier) gunBarrier.ControlStateObject(true);*/
    }

    private async void EntrySpawnGuns() {
        await Task.Delay(5000);

        List<Type> dontDuplicateTypes = new List<Type>();
        foreach (var point in possiblySpawnedPoints) {
            var randomType = randomTypes.GetRandomType(dontDuplicateTypes);
            
            var guns = NetworkPoller.Instance.GetObjects(OwnerClientId, ObjectPollTypes.Guns,randomType, countPerType);
            bool reflection = false;
            foreach (var gun in guns) {
                var gunPosition = reflection ? MirrorReflectionPosition(point) : point;
                DisplayGunClientRpc(gun.objectId,gunPosition.position);
                
                reflection = true;
                
                gunsPositions.Add(gun.objectId,gunPosition);
            }

            reflection = false;
            dontDuplicateTypes.Add(randomType);
        }   
    }

    private Transform MirrorReflectionPosition(Transform reflectionTransform) {
        GameObject spawnObj = new GameObject("SpawnPoint");
        spawnObj.transform.parent = transform;
        spawnObj.transform.localPosition = Vector3.zero;
        spawnObj.transform.localRotation = Quaternion.identity;

        spawnObj.transform.position = new Vector3(reflectionTransform.transform.position.x * -1,
            reflectionTransform.transform.position.y, reflectionTransform.transform.position.z * -1);
        return spawnObj.transform;
    }

    
    [ClientRpc]
    private void DisplayGunClientRpc(int index, Vector3 position) {
        var gun = NetworkPoller.Instance.GetActiveObject(OwnerClientId, ObjectPollTypes.Guns,index);
        gun.transform.position = position + Vector3.up;
        gun.transform.rotation = gunRotation;
        gun.gameObject.SetActive(true);

        var gunBarrier = gun.gameObject.GetComponentInChildren<GunBarrierHandler>();
        if (gunBarrier) gunBarrier.ControlStateObject(true);
    }
    
}
