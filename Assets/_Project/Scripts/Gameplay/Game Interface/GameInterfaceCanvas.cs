
using System;
using UnityEngine;

public class GameInterfaceCanvas : MonoBehaviour
{
    public static GameInterfaceCanvas Instance;
    
    private void Awake() {
        Instance = this;
    }
}
