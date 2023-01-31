
using Unity.Netcode;
using UnityEngine;

public class PlayerGunInvoker : NetworkBehaviour
{
    /*private Gun gun;
    public void InvokeGun(ulong senderId, float x,float y ,float z) {
        SpawnBulletServerRpc(senderId,x,y,z);
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void SpawnBulletServerRpc(ulong senderId, float x,float y ,float z) {
        var spawnedBullet = Spawner.Instance.SpawnNetworkObject(gun.GetGunBullet(), gun.GetShootPoint().position, Quaternion.identity);
        spawnedBullet.gameObject.SetActive(false);
        var bulletId = spawnedBullet.GetComponent<NetworkObject>().NetworkObjectId;
        SpawnBulletClientRpc(senderId,bulletId,x,y,z);
    }
    [ClientRpc]
    private void SpawnBulletClientRpc(ulong senderId, ulong bulletId, float x,float y ,float z) {
        if (senderId != OwnerClientId) return;
        
        Debug.Log("try fire");
        gun.TryFire(senderId,bulletId,new Vector3(x,y,z));
    }*/
    
  
}
