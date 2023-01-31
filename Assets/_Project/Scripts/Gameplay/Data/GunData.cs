using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunData : MonoBehaviour
{
    public static GunData Instance;

    private void Awake() {
        Instance = this;
    }
    
    [SerializeField]
    private List<Gun> listGuns = new List<Gun>();

    public Gun GetGun(int index) {
        return listGuns[index];
    }
}
