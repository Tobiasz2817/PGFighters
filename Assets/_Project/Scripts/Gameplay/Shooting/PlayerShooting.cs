using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : NetworkBehaviour
{
    #region Varriables
    
    [SerializeField] private Gun gun;
    [SerializeField] private GunPickUp pickUpGun;
    private Vector3 direction;
    
    [SerializeField] private Animator anim;

    #endregion
    #region UnityEvents

    private void Awake() {
        pickUpGun = GetComponent<GunPickUp>();
    }

    #endregion
    #region Network Callbacks

    public override void OnNetworkSpawn() {
        if (!IsOwner) {
            this.enabled = false;
            return;
        }
        
        pickUpGun.OnGunPickUp += SetGunReferences;
        InputManager.Input.KeyboardCharacter.Shoot.performed += InputShoot;
        // InputManager.Input.KeyboardCharacter.TestShoot.performed += Reversing;

        CameraRay.OnDetected += ReadDirection;
    }

    public override void OnNetworkDespawn() {
        if (!IsOwner) return;
        
        pickUpGun.OnGunPickUp += SetGunReferences;
        InputManager.Input.KeyboardCharacter.Shoot.performed -= InputShoot;
        // InputManager.Input.KeyboardCharacter.TestShoot.performed -= Reversing;
        CameraRay.OnDetected -= ReadDirection;
    }

    #endregion
    #region Animations Callbacks

    [ServerRpc(RequireOwnership = false)]
    private void AnimShootServerRpc(string nameTrigger) {
        AnimShootClientRpc(nameTrigger);
    }
    [ClientRpc]
    private void AnimShootClientRpc(string nameTrigger) {
        if (IsOwner) return;
        AnimShoot(nameTrigger);
    }

    private void AnimShoot(string nameTrigger) {
        anim.SetTrigger(nameTrigger);
    }
    public void Shooting() {
        if (gun != null) {
            var bulletId = NetworkPoller.Instance.GetIndexObject(OwnerClientId,gun.GetGunBullet().Type,gun.GetGunBullet().GetType());
            Shoot(OwnerClientId,bulletId,direction);
        }
    }

    #endregion
    #region Shooting
    
    private void ShootServerRpc(ulong ownerId,int bulletId, Vector3 direction) {
        gun.TryFire(ownerId,bulletId,direction);
    }
    
    private void ShootClientRpc(ulong ownerId,int bulletId, Vector3 direction) {
        gun.TryFire(ownerId,bulletId,direction);
    }

    private void Shoot(ulong ownerId,int bulletId, Vector3 direction) {
        gun.TryFire(ownerId,bulletId,direction);
    }
    
    

    #endregion
    #region Reading Input Handlers

    private void InputShoot(InputAction.CallbackContext inputs) {
        if (!IsOwner) return;
        if (GameManager.Instance.CurrentState != GameManager.GameState.Started) return;
        
        if (gun != null) {
            if (gun.CanShoot) {
                if (gun.animateShoot) {
                    AnimShootServerRpc("Shoot");
                    AnimShoot("Shoot");
                }
                else {
                    
                }
            }
        }
    }
    
    private void ReadDirection(RaycastHit hit) {
        direction = hit.point;
    }

    #endregion
    #region Gun Ref
    
    private void SetGunReferences(ulong ownerId,int index) {
        var gun_ = (Gun)NetworkPoller.Instance.GetActiveObject(ownerId, ObjectPollTypes.Guns, index);
        if (gun_ == null) Debug.Log("Gun Reference are null");
        this.gun = gun_;
    }
    
    public Gun GetGunReference() {
        return gun;
    }
    

    #endregion
}
