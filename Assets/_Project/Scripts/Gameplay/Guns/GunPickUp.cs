using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GunPickUp : NetworkBehaviour
{
    public event Action<ulong,int> OnGunPickUp;
    public static event Action<ulong,int> OnPlayerGunPickUp;

    public override void OnNetworkSpawn() {
        enabled = IsOwner;
        if (!IsOwner) return;
        //NetworkPoller.Instance.OnPollerInstances += EntryEquipGun;
    }
    

    public override void OnNetworkDespawn() {
        if (!NetworkPoller.Instance) return;
        
        //NetworkPoller.Instance.OnPollerInstances -= EntryEquipGun;
    }
    
    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Gun")) return;

        if (other.TryGetComponent(out GunBarrierHandler gunBarrierHandler)) {
            gunBarrierHandler.ControlStateObject(false);
            //NetworkPoller.Instance.PollObject(gunBarrierHandler.polledObject.ownerId,ObjectPollTypes.Guns,gunBarrierHandler.polledObject.objectId);
            PickGunServerRpc(gunBarrierHandler.polledObject.ownerId,gunBarrierHandler.polledObject.objectId);
            PickGun(gunBarrierHandler.polledObject.ownerId,gunBarrierHandler.polledObject.objectId);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void PickGunServerRpc(ulong ownerId,int index) {
        PickGunClientRpc(ownerId,index);
    }
    [ClientRpc]
    private void PickGunClientRpc(ulong ownerId,int index) {
        if (IsOwner) return;
        PickGun(ownerId,index);
    }
    private void PickGun(ulong ownerId,int index) {
        OnGunPickUp?.Invoke(ownerId,index);
        OnPlayerGunPickUp?.Invoke(ownerId,index);
    }
    
    private void EntryEquipGun() {
        var index = NetworkPoller.Instance.GetObjectByGenericType<BasicGun>(OwnerClientId,ObjectPollTypes.Guns);
        PickGunServerRpc(OwnerClientId,index.objectId);
        PickGun(OwnerClientId,index.objectId);
    }
}
