using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Utilities;

public class LoadingSceneManager : SingletonPersistent<LoadingSceneManager>
{
    public void Init() {
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= ClientsLoadedEvent;
        NetworkManager.Singleton.SceneManager.OnLoadComplete -= ClientsLoadedScene;
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += ClientsLoadedEvent;
        NetworkManager.Singleton.SceneManager.OnLoadComplete += ClientsLoadedScene;
    }

    public event Action<List<ulong>> OnClientsConnected;
    public event Action<ulong> OnClientConnected;
    
    private void ClientsLoadedEvent(string scenename, LoadSceneMode loadscenemode, List<ulong> clientscompleted, List<ulong> clientstimedout) {
        OnClientsConnected?.Invoke(clientscompleted);
    }

    private void ClientsLoadedScene(ulong clientid, string scenename, LoadSceneMode loadscenemode) {
        OnClientConnected?.Invoke(clientid);
    }
}
