using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRay : MonoBehaviour
{
    [SerializeField] private LayerMask rayMask;

    private PlayerInput rayInput;
    private Vector3 mousePointOnGround = Vector3.zero;

    public static event Action<RaycastHit> OnDetected;
    
    private void Awake() {
        rayInput = GetComponent<PlayerInput>();
        
    }
    
    private void OnEnable() {
        rayInput.currentActionMap["Mouse"].performed += ReadMousePosition;
    }
    
    private void ReadMousePosition(InputAction.CallbackContext obj) {
        Ray ray = Camera.main.ScreenPointToRay(obj.ReadValue<Vector2>());
        
        RaycastHit info;
        if (Physics.Raycast(ray, out info,1000,rayMask)) {
            mousePointOnGround = ray.direction * Vector3.Distance(ray.origin,info.point);

            OnDetected?.Invoke(info);
        }
    }

    private void Update() {
        Debug.DrawRay(Camera.main.transform.position,mousePointOnGround,Color.black);
    }
}
