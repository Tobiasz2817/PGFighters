using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

public class CameraRay : Singleton<CameraRay>
{
    [SerializeField] private LayerMask rayMask;

    private Vector3 mousePointOnGround = Vector3.zero;
    private Vector2 mouseLastInput;

    public static event Action<RaycastHit> OnDetected;

    private void OnEnable() {
        InputManager.Input.Environment.Mouse.performed += ReadMousePosition;
    }

    private void OnDisable() {
        InputManager.Input.Environment.Mouse.performed -= ReadMousePosition;
    }

    private void ReadMousePosition(InputAction.CallbackContext obj) {
        mouseLastInput = obj.ReadValue<Vector2>();
        var raycast = GetRay(mouseLastInput,rayMask,0f);
        mousePointOnGround = raycast.point;
        OnDetected?.Invoke(raycast);
    }
    
    public RaycastHit GetRay(float posY = 0f) {
        return MakeRay(mouseLastInput,rayMask,posY);
    }
    public RaycastHit GetRay(LayerMask layerMask, float posY = 0f) {
        return MakeRay(mouseLastInput,layerMask,posY);
    }
    
    public RaycastHit GetRay(Vector3 pos,LayerMask layerMask, float posY = 0f) {
        return MakeRay(pos,layerMask,posY);
    }

    private RaycastHit MakeRay(Vector3 pos,LayerMask layerMask, float posY = 0f) {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        
        RaycastHit info;
        if (Physics.Raycast(ray, out info,1000,layerMask)) {
            var point = new Vector3(info.point.x,posY,info.point.z);
            var distance = Vector3.Distance(ray.origin,point);
            info.point = ray.GetPoint(distance);
        }

        return info;
    }
    
    private void Update() {
        Debug.DrawLine(Camera.main.transform.position,mousePointOnGround,Color.black);
    }
}
