using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerBulletPoller : NetworkBehaviour
{
    [SerializeField] private ObjectPoller objectPollerPrefab;
    public ObjectPoller ObjectPoller { private set; get; }

    public override void OnNetworkSpawn() {
        if (!IsOwner) return;
        
        PlayerCustomizeGun.OnGunChanged += CreatePoller;
    }
    public override void OnNetworkDespawn() {
        if (!IsOwner) return;
        
        PlayerCustomizeGun.OnGunChanged -= CreatePoller;
    }

    private void CreatePoller(Gun gun) {
        CreatePollerServerRpc(gun.gunId, OwnerClientId);
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void CreatePollerServerRpc(int indexGun, ulong clientId) {
        var poller = Spawner.Instance.SpawnNetworkObjectWithOwnership(objectPollerPrefab, clientId);
        ObjectPoller = poller;
        poller.InitPoller(GunData.Instance.GetGun(indexGun).GetGunBullet().gameObject,5);
        poller.CreatePoller();
    }

}
