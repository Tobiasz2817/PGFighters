using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerCustomizeGun : NetworkBehaviour
{
    public PlayerEquipmentData playerEquipmentData { private set; get; }
    public PlayerShooting playerShooting { private set; get; }
    public Gun currentGun { private set; get; }
    public static event Action<Gun> OnGunChanged; 

    private void Awake() {
        playerEquipmentData = GetComponent<PlayerEquipmentData>();
        playerShooting = GetComponent<PlayerShooting>();
    }

    public override void OnNetworkSpawn() {
        if (!IsOwner) return;

        EquipGunServerRpc("+ R Hand",0);
    }
    

    [ServerRpc(RequireOwnership = false)]
    private void EquipGunServerRpc(string content,int index) {
        EquipGunClientRpc(content,index);
    }
    
    [ClientRpc]
    private void EquipGunClientRpc(string content,int index) {
        currentGun = Instantiate( GunData.Instance.GetGun(index), playerEquipmentData.GetContent(content));
        currentGun.transform.localPosition = Vector3.zero;
        currentGun.transform.localRotation = Quaternion.identity;
        currentGun.gunId = index;
        
        playerShooting.SetGunReferences(currentGun);
        OnGunChanged?.Invoke(currentGun);
        Debug.Log(currentGun.transform.name);
    }
}
