using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReference : MonoBehaviour
{
    [field: SerializeField] public PlayerHealth playerHealth { private set; get; }
}
