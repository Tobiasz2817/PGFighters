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
    [SerializeField] private GameObject robotPrefab;
    [SerializeField] private Transform[] points;
    
    public override void OnNetworkDespawn() {
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= ClientsLoadedEvent;
        NetworkManager.Singleton.SceneManager.OnLoadComplete -= ClientsLoadedScene;
    }

    private void OnEnable() {
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += ClientsLoadedEvent;
        NetworkManager.Singleton.SceneManager.OnLoadComplete += ClientsLoadedScene;
    }
    
    private void OnClientDisconnected(ulong disconnectId) {
        Debug.Log("Id: " + disconnectId);
    }

    private void ClientsLoadedEvent(string scenename, LoadSceneMode loadscenemode, List<ulong> clientscompleted, List<ulong> clientstimedout) {
        Debug.Log("ClientsLoadedEvent");
        if (!IsServer) return;
        SpawnPlayers();
        Debug.Log("Car is spawned");
    }

    private void ClientsLoadedScene(ulong clientid, string scenename, LoadSceneMode loadscenemode) {
        Debug.Log("ClientsLoadedScene");
    }
    private void SpawnPlayers() {
        foreach (var client in NetworkManager.Singleton.ConnectedClients) {
            Debug.Log(NetworkManager.Singleton.ConnectedClients);
            SpawnRobot(client.Key);
        }
    }

    private void SpawnRobot(ulong id) {
        if (NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(id) != null) return;
        
        Spawner.Instance.SpawnPlayer(robotPrefab, points[id].position, points[id].rotation, id);
    }
}
