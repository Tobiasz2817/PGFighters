using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimInvoke : MonoBehaviour
{
   [SerializeField] private PlayerShooting _playerShooting;

   public void InvokeShoot() {
      _playerShooting.Shooting();
   }
}
