using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerBulletPoller : NetworkBehaviour
{
    [SerializeField] private ObjectPoller objectPollerPrefab;
    [SerializeField] private int countStartedBullets = 15;
    [field: SerializeField] 
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
        poller.InitPoller(GunData.Instance.GetGun(indexGun).GetGunBullet().gameObject,countStartedBullets);
        poller.CreatePoller(NetworkObjectId);

        SetRefPollerClientRpc(poller.NetworkObjectId);
    }
    
    [ClientRpc]
    private void SetRefPollerClientRpc(ulong pollerId) {
        var pollerRef = NetworkManager.Singleton.SpawnManager.SpawnedObjects[pollerId].GetComponent<ObjectPoller>();
        ObjectPoller = pollerRef;
    }

}
