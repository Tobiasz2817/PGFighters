using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestSpawner : MonoBehaviour
{
    public LayerMask layerMask;
    public GameObject bullet;
    private void Awake() {
        InputManager.Input.KeyboardCharacter.SemiShoot.performed += SpawnBullet;
    }

    private void SpawnBullet(InputAction.CallbackContext obj) {
        var rayPoint = CameraRay.Instance.GetRay(layerMask,10f);
        Instantiate(bullet, rayPoint.point,Quaternion.identity);
    }
}
