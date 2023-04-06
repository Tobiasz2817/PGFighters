using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float smoothTime = 0.01f;
    
    private Vector3 inputMovement;
    private Vector3 rayPoint;
    
    //private Rigidbody rigidbody;

    private Vector3 currentMovement;
    private Vector3 targetMovement;

    public override void OnNetworkSpawn() {
        if(!IsOwner) this.enabled = false;

        InputManager.Input.KeyboardCharacter.Movement.performed += ReadMovement;
        CameraRay.OnDetected += ReadRayVector3;
    }

    public override void OnNetworkDespawn() {
        if (!IsOwner) return;
        
        InputManager.Input.KeyboardCharacter.Movement.performed -= ReadMovement;
        CameraRay.OnDetected -= ReadRayVector3;
    }

    private void Update() {
        if (!GameManager.Instance || GameManager.Instance.CurrentState != GameManager.GameState.Started) return;
        
        RotateCharacter();
        MoveCharacter();
    }

    private void RotateCharacter() {
        transform.LookAt(rayPoint);
    }

    private void MoveCharacter() {
        if (inputMovement == Vector3.zero) return;
        
        inputMovement.y = 0;
        targetMovement = inputMovement.normalized;

        currentMovement = Vector3.SmoothDamp(currentMovement,  targetMovement,ref currentMovement, smoothTime);
        transform.position += currentMovement * (speed * Time.deltaTime);
    }
    private void ReadMovement(InputAction.CallbackContext obj) {
        inputMovement = obj.ReadValue<Vector3>();
    }
    private void ReadRayVector3(RaycastHit raycastHit) {
        rayPoint = raycastHit.point;
    }
}
