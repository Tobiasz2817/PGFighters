using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    public static UICanvas Instance;
    
        private void Awake() {
            Instance = this;
        }
}
