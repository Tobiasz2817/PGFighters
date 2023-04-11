using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateCharacter : NetworkBehaviour
{
    [SerializeField] private Animator anim;

    private NetworkVariable<Vector3> inputMovement = new NetworkVariable<Vector3>(default,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
    public override void OnNetworkSpawn() {
        if (!IsOwner) 
            return;
        
        InputManager.Input.KeyboardCharacter.Movement.performed += ReadMovement;
    }
    
    public override void OnNetworkDespawn() {
        if (!IsOwner) 
            return;
        
        InputManager.Input.KeyboardCharacter.Movement.performed -= ReadMovement;
    }

    private void Update() {
        var velocityZ = Vector3.Dot(inputMovement.Value.normalized, transform.forward);
        var velocityX = Vector3.Dot(inputMovement.Value.normalized, transform.right);
        UpdateAnim(velocityX, velocityZ);
    }

    private void UpdateAnim(float velX, float velZ) {
        anim.SetFloat("MoveZ",velZ,0.1f,Time.deltaTime);
        anim.SetFloat("MoveX",velX,0.1f,Time.deltaTime);
    }
    
    private void ReadMovement(InputAction.CallbackContext obj) {
        inputMovement.Value = obj.ReadValue<Vector3>();
    }

}
