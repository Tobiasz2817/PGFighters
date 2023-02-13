
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private Gun gun;
    private PlayerInput playerInput;
    private Vector3 direction;
    public override void OnNetworkSpawn() {
        if (!IsOwner) {
            this.enabled = false;
            return;
        }
        
        playerInput.currentActionMap["Shoot"].performed += Shoot;
        CameraRay.OnDetected += ReadDirection;
    }

    public override void OnNetworkDespawn() {
        if (!IsOwner) return;

        playerInput.currentActionMap["Shoot"].performed -= Shoot;
        CameraRay.OnDetected -= ReadDirection;
    }

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        gun = GetComponent<Gun>();
    }

    private void Shoot(InputAction.CallbackContext inputs) {
        if (!IsOwner) return;
        if (GameManager.Instance.GetGameState() != GameManager.GameState.Started) return;
        if (gun.CanShoot) {
            ShootBullet();
            Debug.Log("I Shooting");
        }
    }

    private void ShootBullet() {
        gun.TryFire(OwnerClientId,direction);
    }
    private void ReadDirection(RaycastHit hit) {
        direction = hit.point;
    }
    public void SetGunReferences(Gun gun_) {
        this.gun = gun_;
    }
    
    public Gun GetGunReference() {
        return gun;
    }
}
