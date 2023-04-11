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

    private void Start() {
        DisableShootInputs();
    }

    #endregion
    #region Network Callbacks

    public override void OnNetworkSpawn() {
        if (!IsOwner) {
            this.enabled = false;
            return;
        }
        
        pickUpGun.OnGunPickUp += SetGunReferences;
        InputManager.Input.KeyboardCharacter.AutomaticShoot.performed += InputShoot;
        InputManager.Input.KeyboardCharacter.SemiShoot.performed += InputShoot;
        InputManager.Input.KeyboardCharacter.TestShoot.performed += InputShootK;
        CameraRay.OnDetected += ReadDirection;
    }

    public override void OnNetworkDespawn() {
        if (!IsOwner) return;
        
        pickUpGun.OnGunPickUp -= SetGunReferences;
        InputManager.Input.KeyboardCharacter.AutomaticShoot.performed -= InputShoot;
        InputManager.Input.KeyboardCharacter.SemiShoot.performed -= InputShoot;
        InputManager.Input.KeyboardCharacter.TestShoot.performed -= InputShootK;
        CameraRay.OnDetected -= ReadDirection;
    }

    #endregion
    #region Animations Callbacks

    [ServerRpc(RequireOwnership = false)]
    private void AnimShootServerRpc(string stateName) {
        AnimShootClientRpc(stateName);
    }
    [ClientRpc]
    private void AnimShootClientRpc(string stateName) {
        if (IsOwner) return;
        AnimShoot(stateName);
    }

    private void AnimShoot(string stateName) {
        anim.Play(stateName, 1, 0f);
        anim.Play(stateName);
    }
    public void Shooting() {
        if (gun != null) {
            var bulletId = NetworkPoller.Instance.GetIndexObject(OwnerClientId,gun.GetGunBullet().Type,gun.GetGunBullet().GetType());
            Shoot(OwnerClientId,bulletId,direction);
        }
    }

    #endregion
    #region Shooting
    
    [ServerRpc(RequireOwnership = false)]
    private void ShootServerRpc(ulong ownerId,int bulletId, Vector3 direction) {
        ShootClientRpc(ownerId,bulletId,direction);
    }
    
    [ClientRpc]
    private void ShootClientRpc(ulong ownerId,int bulletId, Vector3 direction) {
        if (IsOwner) return;
        Shoot(ownerId, bulletId, direction);
    }

    private void Shoot(ulong ownerId, int bulletId, Vector3 direction) {
        if (gun != null) 
            gun.TryFire(ownerId, bulletId, direction);
    }
    
    

    #endregion
    #region Reading Input Handlers

    private void InputShoot(InputAction.CallbackContext inputs) {
        if (!IsOwner) return;
        if (GameManager.Instance.CurrentState != GameManager.GameState.Started) return;
        Debug.Log("InputShootInvoke");
        
        if (gun != null) {
            if (gun.CanShoot) {
                if (gun.animateShoot) {
                    AnimShootServerRpc("Attack01");
                    AnimShoot("Attack01");
                }
                else {
                    var bulletId = NetworkPoller.Instance.GetIndexObject(OwnerClientId,gun.GetGunBullet().Type,gun.GetGunBullet().GetType());
                    ShootServerRpc(OwnerClientId,bulletId,direction);
                    Shoot(OwnerClientId,bulletId,direction);
                }
            }
        }
    }
    
    private void InputShootK(InputAction.CallbackContext obj) {
        if (!IsOwner) return;
        if (GameManager.Instance.CurrentState != GameManager.GameState.Started) return;
        
        if (gun != null) {
            if (gun.CanShoot) {
                if (gun.animateShoot) {
                    AnimShootServerRpc("Attack02");
                    AnimShoot("Attack02");
                }
                else {
                    var bulletId = NetworkPoller.Instance.GetIndexObject(OwnerClientId,gun.GetGunBullet().Type,gun.GetGunBullet().GetType());
                    ShootServerRpc(OwnerClientId,bulletId,direction);
                    Shoot(OwnerClientId,bulletId,direction);
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
        if (gun_ == null)
        {
            Debug.Log("Gun Reference are null");
            return;
        }
        
        this.gun = gun_;
        EnableInputsBasedOnTypeFire(gun.ShootType);
    }
    
    public Gun GetGunReference() {
        return gun;
    }
    
    #endregion

    #region ModifyInputs

    private void EnableInputsBasedOnTypeFire(TypeShooting typeShooting) {
        switch (typeShooting)
        {
            case TypeShooting.Automatic:
            {
                InputManager.Input.KeyboardCharacter.SemiShoot.Disable();
                InputManager.Input.KeyboardCharacter.AutomaticShoot.Enable();
            }
                break;
            case TypeShooting.Semi:
            {
                InputManager.Input.KeyboardCharacter.AutomaticShoot.Disable();
                InputManager.Input.KeyboardCharacter.SemiShoot.Enable();
            }
                break;
        }
    }

    private void DisableShootInputs() {
        InputManager.Input.KeyboardCharacter.AutomaticShoot.Disable();
        InputManager.Input.KeyboardCharacter.SemiShoot.Disable();
    }

    #endregion
}
