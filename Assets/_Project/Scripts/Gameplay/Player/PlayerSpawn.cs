using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Multiplayer.Samples.Utilities;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawn : NetworkBehaviour
{
    [SerializeField] private NetworkObject robotPrefab;
    [SerializeField] private Transform[] points;

    public static event Action<List<ulong>> OnPlayersSpawned;

    public override void OnNetworkDespawn() {
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;

        LoadingSceneManager.Instance.OnClientsConnected -= SpawnPlayers;
    }

    private void OnEnable() {
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        
        LoadingSceneManager.Instance.OnClientsConnected += SpawnPlayers;
    }
    
    private void OnClientDisconnected(ulong disconnectId) {
        Debug.Log("Id: " + disconnectId);
    }
    
    private void SpawnPlayers(List<ulong> clientscompleted) {
        if (!IsServer) return;
        
        var list = new List<ulong>();
        foreach (var client in clientscompleted) {
            Debug.Log("Id: + " + client);
            var robot = SpawnRobot(client);
            if(robot != 250)
                list.Add(robot);
        }
        
        OnPlayersSpawned?.Invoke(list);
    }

    private ulong SpawnRobot(ulong id) {
        var obj = Spawner.Instance.SpawnPlayer(robotPrefab, points[id].position, points[id].rotation, id);
        return obj.OwnerClientId;
    }
}
