using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRay : MonoBehaviour
{
    [SerializeField] private LayerMask rayMask;

    private Vector3 mousePointOnGround = Vector3.zero;

    public static event Action<RaycastHit> OnDetected;

    private void OnEnable() {
        InputManager.Input.Environment.Mouse.performed += ReadMousePosition;
    }

    private void OnDisable() {
        InputManager.Input.Environment.Mouse.performed -= ReadMousePosition;
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
