
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
        if (GameManager.Instance.GetGameState() != GameManager.GameState.Started) return;
        
        SpawnBulletServerRpc(OwnerClientId);
        
        Debug.Log("Dir Bullet: " + direction);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnBulletServerRpc(ulong clientId) {
        var playerShooting = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientId).GetComponent<PlayerShooting>();
        var shootPoint = playerShooting.gun.GetShootPoint();
        var bulletReference = playerShooting.gun.GetGunBullet();

        var spawnedBullet = Spawner.Instance.SpawnNetworkObjectWithOwnership(bulletReference, shootPoint.position, Quaternion.identity,clientId);
        ShootClientRpc(clientId,spawnedBullet.NetworkObjectId);

    }
    [ClientRpc]
    private void ShootClientRpc(ulong clientId, ulong bulletId) {
        if (OwnerClientId != clientId || !IsOwner) return;
        gun.TryFire(clientId,bulletId,direction);
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
