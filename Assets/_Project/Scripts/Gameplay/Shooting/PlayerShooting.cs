using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : NetworkBehaviour
{
    #region Varriables
    
    [SerializeField] private Gun gun;
    [SerializeField] private GunPickUp pickUpGun;

    private Vector3 shootDirection;

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
        pickUpGun.OnGunPickUp += SetGunReferences;

        this.enabled = IsOwner;
        if (!IsOwner) return;

        InputManager.Input.KeyboardCharacter.AutomaticShoot.performed += InputShoot;
        InputManager.Input.KeyboardCharacter.SemiShoot.performed += InputShoot;
        InputManager.Input.KeyboardCharacter.TestShoot.performed += InputShootK;
    }

    public override void OnNetworkDespawn() {
        pickUpGun.OnGunPickUp -= SetGunReferences;
        
        if (!IsOwner) return;
        
        InputManager.Input.KeyboardCharacter.AutomaticShoot.performed -= InputShoot;
        InputManager.Input.KeyboardCharacter.SemiShoot.performed -= InputShoot;
        InputManager.Input.KeyboardCharacter.TestShoot.performed -= InputShootK;
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
            Shoot(OwnerClientId,bulletId,shootDirection);
        }
    }

    #endregion
    #region Shooting
    
    [ServerRpc(RequireOwnership = false)]
    private void ShootServerRpc(ulong ownerId,int bulletId, Vector3 shootDirection) {
        ShootClientRpc(ownerId,bulletId,shootDirection);
    }
    
    [ClientRpc]
    private void ShootClientRpc(ulong ownerId,int bulletId, Vector3 shootDirection) {
        if (IsOwner) return;
        Shoot(ownerId, bulletId,shootDirection);
    }

    private void Shoot(ulong ownerId, int bulletId, Vector3 shootDirection) {
        Debug.Log(gun);
        if (gun != null) 
            gun.TryFire(ownerId, bulletId, shootDirection);
    }
    
    

    #endregion
    #region Reading Input Handlers

    private void InputShoot(InputAction.CallbackContext inputs) {
        if (!IsOwner) return;
        if (GameManager.Instance.CurrentState != GameManager.GameState.Started) return;
        
        if (gun != null) {
            if (gun.CanShoot) {
                shootDirection = CameraRay.Instance.GetRay(gun.GetShootPoint().position.y).point;
                
                if (gun.animateShoot) {
                    SetDirectionServerRpc(shootDirection);
                    SetDirection(shootDirection);
                    AnimShootServerRpc("Attack01");
                    AnimShoot("Attack01");
                }
                else {
                    var bulletId = NetworkPoller.Instance.GetIndexObject(OwnerClientId,gun.GetGunBullet().Type,gun.GetGunBullet().GetType());
                    
                    ShootServerRpc(OwnerClientId,bulletId,shootDirection);
                    Shoot(OwnerClientId,bulletId,shootDirection);
                }
            }
        }
    }
    
    private void InputShootK(InputAction.CallbackContext obj) {
        if (!IsOwner) return;
        if (GameManager.Instance.CurrentState != GameManager.GameState.Started) return;
        
        if (gun != null) {
            if (gun.CanShoot) {
                shootDirection = CameraRay.Instance.GetRay(gun.GetShootPoint().position.y).point;
                if (gun.animateShoot) {
                    SetDirectionServerRpc(shootDirection);
                    SetDirection(shootDirection);
                    AnimShootServerRpc("Attack02");
                    AnimShoot("Attack02");
                }
                else {
                    var bulletId = NetworkPoller.Instance.GetIndexObject(OwnerClientId,gun.GetGunBullet().Type,gun.GetGunBullet().GetType());
                    
                    ShootServerRpc(OwnerClientId,bulletId,shootDirection);
                    Shoot(OwnerClientId,bulletId,shootDirection);
                }
            }
        }
    }

    #endregion

    [ServerRpc(RequireOwnership = false)]
    private void SetDirectionServerRpc(Vector3 direction) {
        SetDirectionClientRpc(direction);
    }
    
    [ClientRpc]
    private void SetDirectionClientRpc(Vector3 direction) {
        if (IsOwner) return;
        SetDirection(direction);
    }
    
    private void SetDirection(Vector3 direction) {
        this.shootDirection = direction;
    }
    
    #region Gun Ref
    
    private void SetGunReferences(ulong ownerId,int index) {
        var gun_ = (Gun)NetworkPoller.Instance.GetActiveObject(0, ObjectPollTypes.Guns, index);
        Debug.Log("Equip Gun");
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
