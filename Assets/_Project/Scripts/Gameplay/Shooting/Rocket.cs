using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Rocket : NetworkBehaviour
{
    private Rigidbody rb;
    
    public override void OnNetworkSpawn() {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other) {
        if(!IsServer) return;

        StateVisiblyServerRpc(false);
    }

    
    [ServerRpc(RequireOwnership = false)]
    private void StateVisiblyServerRpc(bool state) {
        StateVisiblyClientRpc(state);
    }
    [ClientRpc]
    private void StateVisiblyClientRpc(bool state) {
        StateVisibly(state);
    }
    
    private void StateVisibly(bool state) {
        gameObject.SetActive(state);
    }

    public void ImpulseRocket(float power,Vector3 direction) {
        rb.AddForce(power * direction, ForceMode.Impulse);
    }
}
