using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerCustomizeGun : NetworkBehaviour
{
    public GunPickUp gunPickUp { private set; get; }
    public PlayerEquipmentData playerEquipmentData { private set; get; }

    private Gun currentGun;

    private void Awake() {
        playerEquipmentData = GetComponent<PlayerEquipmentData>();
        gunPickUp = GetComponent<GunPickUp>();
    }

    public override void OnNetworkSpawn() {
        gunPickUp.OnGunPickUp += EquipGun;
    }
    
    public override void OnNetworkDespawn() {
        gunPickUp.OnGunPickUp -= EquipGun;
    }

    private void EquipGun(ulong ownerId,int index) {
        TryPollCurrentGun();
        EquipGun("+ R Hand",index,ownerId);
    }
    

    [ServerRpc(RequireOwnership = false)]
    private void EquipGunServerRpc(string content,int index, ulong ownerId) {
        EquipGunClientRpc(content,index,ownerId);
    }
    
    [ClientRpc]
    private void EquipGunClientRpc(string content,int index, ulong ownerId) {
        EquipGun(content,index,ownerId);
    }
    private void EquipGun(string content,int index, ulong ownerId) {
        Debug.Log("Equip gun");
        currentGun = (Gun)NetworkPoller.Instance.GetActiveObject(0, ObjectPollTypes.Guns,index);
        var gunBarrierHandler = currentGun.transform.GetComponentInChildren<GunBarrierHandler>();
        if (gunBarrierHandler) gunBarrierHandler.ControlStateObject(false);
        Debug.Log(ownerId);
        currentGun.ReverseBullets(ownerId);
        currentGun.transform.parent = playerEquipmentData.GetContent(content);
        currentGun.transform.localPosition = Vector3.zero;
        currentGun.transform.localRotation = Quaternion.identity;
        currentGun.gameObject.SetActive(true);
    }

    private void TryPollCurrentGun() {
        if (!currentGun) return;
        NetworkPoller.Instance.PollObject(currentGun);
    }
}
