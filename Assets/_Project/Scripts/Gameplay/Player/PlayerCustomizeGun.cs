using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCustomizeGun : NetworkBehaviour
{
    public PlayerEquipmentData playerEquipmentData { private set; get; }
    public PlayerShooting playerShooting { private set; get; }
    public Gun currentGun { private set; get; }

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

        playerShooting.SetGunReferences(currentGun);
        Debug.Log(currentGun.transform.name);
    }
}
