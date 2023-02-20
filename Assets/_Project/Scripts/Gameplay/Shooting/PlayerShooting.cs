
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private Gun gun;
    private PlayerInput playerInput;
    private Vector3 direction;
    
    [SerializeField] private Rocket rocket;
    [SerializeField] public Transform rocketShootPoint;
    [SerializeField] private float rocketPower;

    [SerializeField] private Animator anim;
    
    public override void OnNetworkSpawn() {
        if (!IsOwner) {
            this.enabled = false;
            return;
        }
        
        playerInput.currentActionMap["Shoot"].performed += Shoot;
        playerInput.currentActionMap["TestShoot"].performed += ShootTest;
        CameraRay.OnDetected += ReadDirection;
    }

    public override void OnNetworkDespawn() {
        if (!IsOwner) return;

        playerInput.currentActionMap["Shoot"].performed -= Shoot;
        playerInput.currentActionMap["TestShoot"].performed -= ShootTest;
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
            AnimTriggerServerRpc("Shoot");
        }
    }
    private void ShootTest(InputAction.CallbackContext inputs) {
        if (!IsOwner) return;
        if (GameManager.Instance.GetGameState() != GameManager.GameState.Started) return;
        ShootRocketServerRpc(rocketPower,direction.x,direction.y,direction.z,OwnerClientId);
    }

    public void Shooting() {
        if (!IsOwner) return;

        ShootBullet();
        Debug.Log("I Shooting");
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShootRocketServerRpc(float power,float x,float y, float z, ulong id) {

        var player = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(id);
        var playerShooting = player.GetComponent<PlayerShooting>();
        
        Vector3 direction = (new Vector3(x,y,z) - playerShooting.rocketShootPoint.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation.z = 0;
        rotation.x = 0;
        var rocket = Spawner.Instance.SpawnNetworkObject(this.rocket, playerShooting.rocketShootPoint.position, rotation);
        rocket.ImpulseRocket(power  * Time.deltaTime, new Vector3(x,y,z));
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

    [ServerRpc(RequireOwnership = false)]
    private void AnimTriggerServerRpc(string nameTrigger) {
        AnimTriggerClientRpc(nameTrigger);
    }
    [ClientRpc]
    private void AnimTriggerClientRpc(string nameTrigger) {
        anim.SetTrigger(nameTrigger);
    }
}
