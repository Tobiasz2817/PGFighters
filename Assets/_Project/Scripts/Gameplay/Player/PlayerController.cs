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

    [SerializeField] private Animator anim;
    
    private Vector3 inputMovement;
    private Vector3 rayPoint;
    
    private PlayerInput playerInput;
    //private Rigidbody rigidbody;

    private Vector3 currentMovement;
    private Vector3 targetMovement;

    float velocityZ;
    float velocityX;

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
    }

    private void Update() {
        if (GameManager.Instance.GetGameState() != GameManager.GameState.Started) return;
        
        RotateCharacter();
        MoveCharacter();
        if (IsOwner) UpdateAnim();
    }

    private void RotateCharacter() {
        transform.LookAt(rayPoint);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void MoveCharacter() {
        if (inputMovement == Vector3.zero) return;
        
        inputMovement.y = 0;
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

    private void UpdateAnim() {
        velocityZ = Vector3.Dot(inputMovement.normalized, transform.forward);
        velocityX = Vector3.Dot(inputMovement.normalized, transform.right);
        
        UpdateAnimServerRpc(velocityX,velocityZ);
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateAnimServerRpc(float moveX, float moveZ) {
        UpdateAnimClientRpc(moveX, moveZ);
    }
    [ClientRpc]
    private void UpdateAnimClientRpc(float velX, float velZ) {
        anim.SetFloat("MoveZ",velZ,0.1f,Time.deltaTime);
        anim.SetFloat("MoveX",velX,0.1f,Time.deltaTime);
    }
}
