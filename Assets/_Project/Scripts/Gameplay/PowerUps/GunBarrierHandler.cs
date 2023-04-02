using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBarrierHandler : MonoBehaviour
{
    public PolledObject polledObject { private set; get; }

    private void Awake() {
        polledObject = GetComponentInParent<PolledObject>();
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Bullet")) return;

        if (other.TryGetComponent(out PolledObject polledObject)) {
            if (NetworkPoller.Instance == null) return;
            NetworkPoller.Instance.PollObject(polledObject.ownerId,polledObject.Type,polledObject.objectId);
        }
    }

    public void ControlStateObject(bool state) {
        gameObject.SetActive(state);
    }
}
