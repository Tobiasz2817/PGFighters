using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class GameplayData : MonoBehaviour, IDataInstances
{
    public static GameplayData Instance;
    void CreateInstance() {
        Instance = this;
        DontDestroyOnLoad(this);
    }
    public Task IsDone() {
        CreateInstance();
        return Task.Delay(1);
    }
}
