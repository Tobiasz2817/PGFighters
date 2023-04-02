
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private Gun gun;
    [SerializeField] private GunPickUp pickUpGun;
    private PlayerInput playerInput;
    private Vector3 direction;
    
    [SerializeField] private Animator anim;

    public override void OnNetworkSpawn() {
        if (!IsOwner) {
            this.enabled = false;
            return;
        }
        
        pickUpGun.OnGunPickUp += SetGunReferences;
        playerInput.currentActionMap["Shoot"].performed += Shoot;
        playerInput.currentActionMap["TestShoot"].performed += Reversing;
        CameraRay.OnDetected += ReadDirection;
    }

    public override void OnNetworkDespawn() {
        if (!IsOwner) return;

        pickUpGun.OnGunPickUp += SetGunReferences;
        playerInput.currentActionMap["Shoot"].performed -= Shoot;
        playerInput.currentActionMap["TestShoot"].performed -= Reversing;
        CameraRay.OnDetected -= ReadDirection;
    }

    private bool tak = false;
    private void Reversing(InputAction.CallbackContext obj) {
        if (!tak) {
            NetworkPoller.Instance.ReversObjectsServerRpc(OwnerClientId,ObjectPollTypes.GunBullets,1);
        }
        else
            NetworkPoller.Instance.ReversObjectsServerRpc(OwnerClientId,ObjectPollTypes.GunBullets,0);

        tak = !tak;
    }

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        gun = GetComponent<Gun>();
        pickUpGun = GetComponent<GunPickUp>();
    }

    private void Shoot(InputAction.CallbackContext inputs) {
        if (!IsOwner) return;
        if (GameManager.Instance.CurrentState != GameManager.GameState.Started) return;
        Debug.Log(gun);

        if (gun != null) {
            if (gun.CanShoot) {
                ShootServerRpc(OwnerClientId,direction);
                Shoot(OwnerClientId,direction);
                Debug.Log("Shooting");
            }
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void ShootServerRpc(ulong ownerId,Vector3 direction) {
        ShootClientRpc(ownerId,direction);
    }
    [ClientRpc]
    private void ShootClientRpc(ulong ownerId,Vector3 direction) {
        if (IsOwner) return;
        Shoot(ownerId,direction);
    }

    private void Shoot(ulong ownerId, Vector3 direction) {
        gun.TryFire(ownerId,direction);
    }
    

    public void Shooting() {
        if (!IsOwner) return;

        ShootBullet();
        Debug.Log("I Shooting");
    }

    private void ShootBullet() {
        gun.TryFire(OwnerClientId,direction);
    }
    private void ReadDirection(RaycastHit hit) {
        direction = hit.point;
    }
    public void SetGunReferences(ulong ownerId,int index) {
        var gun_ = (Gun)NetworkPoller.Instance.GetActiveObject(ownerId, ObjectPollTypes.Guns, index);
        if (gun_ == null) Debug.Log("Gun Reference are null");
        this.gun = gun_;
    }
    
    public Gun GetGunReference() {
        return gun;
    }

    [ServerRpc(RequireOwnership = false)]
    private void AnimTriggerServerRpc(string nameTrigger) {
        AnimTriggerClientRpc(nameTrigger);
    }
    [ClientRpc]
    private void AnimTriggerClientRpc(string nameTrigger) {
        anim.SetTrigger(nameTrigger);
    }
}
