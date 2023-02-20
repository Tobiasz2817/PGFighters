using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class PlayerReferenceCamera : NetworkBehaviour
{
    public override void OnNetworkSpawn() {
        if (!IsOwner) return;
        
        SetCineMachineToPlayer();
    }

    private void SetCineMachineToPlayer() {
        FindObjectOfType<CinemachineVirtualCamera>().LookAt = transform;
        FindObjectOfType<CinemachineVirtualCamera>().Follow = transform;
    }
}
