using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float smoothTime = 0.01f;
    
    private Vector3 inputMovement;
    private Vector3 rayPoint;
    
    private PlayerInput playerInput;
    private Rigidbody rigidbody;

    private Vector3 currentMovement;
    private Vector3 targetMovement;

    public override void OnNetworkSpawn() {
        if(!IsOwner) this.enabled = false;
        Debug.Log("On Network Spawn");
        
        playerInput.currentActionMap["Movement"].performed += ReadMovement;
        CameraRay.OnDetected += ReadRayVector3;
    }

    public override void OnNetworkDespawn() {
        if (!IsOwner) return;
        
        playerInput.currentActionMap["Movement"].performed -= ReadMovement;
        CameraRay.OnDetected -= ReadRayVector3;
    }

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update() {
        if (GameManager.Instance.GetGameState() != GameManager.GameState.Started) return;
        
        RotateCharacter();
        MoveCharacter();
    }

    private void RotateCharacter() {
        transform.LookAt(rayPoint);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void MoveCharacter() {
        if (inputMovement == Vector3.zero) return; 
        //rigidbody.MovePosition((rigidbody.position + inputMovement.normalized) * speed * Time.deltaTime);
        rigidbody.position = (rigidbody.position + inputMovement.normalized * (speed * Time.deltaTime));
        Debug.Log("I Move");
        return;
        inputMovement.y = transform.position.y;
        targetMovement = inputMovement.normalized;

        currentMovement = Vector3.SmoothDamp(currentMovement,  targetMovement,ref currentMovement, smoothTime);
        transform.position += currentMovement * speed * Time.deltaTime;
        
    }

    private void ReadMovement(InputAction.CallbackContext obj) {
        Debug.Log( " I Reading Movement: " + transform.name  + " IsOwner: " + IsOwner);
        inputMovement = obj.ReadValue<Vector3>();
    }
    private void ReadRayVector3(RaycastHit raycastHit) {
        rayPoint = raycastHit.point;
    }
}
