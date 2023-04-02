using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateCharacter : NetworkBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private PlayerInput playerInput;
    
    public override void OnNetworkSpawn() {
        if (!IsOwner) {
            this.enabled = false;
            return;
        }
        playerInput.currentActionMap["Movement"].performed += ReadMovement;
        
        //InputManager.Inputs.Player.Movement.performed += 
    }


    public override void OnNetworkDespawn() {
        if (!IsOwner) return;
        playerInput.currentActionMap["Movement"].performed -= ReadMovement;
    }

    // Update is called once per frame

    [ServerRpc(RequireOwnership = false)]
    private void UpdateAnimServerRpc(float moveX, float moveZ) {
        UpdateAnimClientRpc(moveX, moveZ);
    }
    [ClientRpc]
    private void UpdateAnimClientRpc(float velX, float velZ) {
        anim.SetFloat("MoveZ",velZ,0.1f,Time.deltaTime);
        anim.SetFloat("MoveX",velX,0.1f,Time.deltaTime);
    }
    
    private void ReadMovement(InputAction.CallbackContext obj) {
        Vector3 inputMovement = obj.ReadValue<Vector3>();
        var velocityZ = Vector3.Dot(inputMovement.normalized, transform.forward);
        var velocityX = Vector3.Dot(inputMovement.normalized, transform.right);
        UpdateAnimServerRpc(velocityX,velocityZ);
    }

}
