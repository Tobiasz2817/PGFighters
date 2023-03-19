using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class PlayerCustomizeEquipment : NetworkBehaviour
{
    private PlayerEquipmentData playerEquipmentData;

    private void Awake() {
        playerEquipmentData = GetComponent<PlayerEquipmentData>();
    }

    public override void OnNetworkSpawn() {
        if (!IsOwner) return;

        if (CustomizeCharacterEquipmentData.Instance == null) return;
        foreach (var customize in CustomizeCharacterEquipmentData.Instance.GetCustomizeSelections())
            if (customize.index != -1) 
                SetEquipmentServerRpc(customize.contentName,customize.index);
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void SetEquipmentServerRpc(string content,int index) {
        SetEquipmentClientRpc(content,index);
    }
    
    [ClientRpc]
    private void SetEquipmentClientRpc(string content,int index) {
        if(content != "+ Color")
            Instantiate(CustomizeCharacterEquipmentData.Instance.GetCustomizePrefab(index), playerEquipmentData.GetContent(content));
        else if (content == "+ Color") {
            var color = Instantiate(CustomizeCharacterEquipmentData.Instance.GetCustomizePrefab(index));
            playerEquipmentData.GetContent(content).GetComponent<SkinnedMeshRenderer>().material.mainTexture =
                color.GetComponent<SkinnedMeshRenderer>().material.mainTexture;
            Destroy(color);
        }
    }
}
