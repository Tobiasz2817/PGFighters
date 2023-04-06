using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateCharacter : NetworkBehaviour
{
    [SerializeField] private Animator anim;
    
    public override void OnNetworkSpawn() {
        if (!IsOwner) {
            this.enabled = false;
            return;
        }

        InputManager.Input.KeyboardCharacter.Movement.performed += ReadMovement;
    }


    public override void OnNetworkDespawn() {
        if (!IsOwner) return;
        InputManager.Input.KeyboardCharacter.Movement.performed -= ReadMovement;
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void UpdateAnimServerRpc(float moveX, float moveZ) {
        UpdateAnimClientRpc(moveX, moveZ);
    }
    [ClientRpc]
    private void UpdateAnimClientRpc(float velX, float velZ) {
        if (IsOwner) return;
        UpdateAnim(velX,velZ);
    }

    private void UpdateAnim(float velX, float velZ) {
        anim.SetFloat("MoveZ",velZ,0.1f,Time.deltaTime);
        anim.SetFloat("MoveX",velX,0.1f,Time.deltaTime);
        
        Debug.Log("Animate: x: " + velX + " Z: " + velZ);
    }
    
    private void ReadMovement(InputAction.CallbackContext obj) {
        Vector3 inputMovement = obj.ReadValue<Vector3>();
        var velocityZ = Vector3.Dot(inputMovement.normalized, transform.forward);
        var velocityX = Vector3.Dot(inputMovement.normalized, transform.right);
        UpdateAnimServerRpc(velocityX,velocityZ);
        UpdateAnim(velocityX, velocityZ);
    }

}
