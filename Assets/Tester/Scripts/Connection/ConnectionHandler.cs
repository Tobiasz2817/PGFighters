using System;
using System.Collections;
using LobbyData;
using SceneAnimation;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace ConnectionServices
{
    public class ConnectionHandler : NetworkBehaviour
    {
        [Serializable] public class StartGameEvent : UnityEvent {}
        [SerializeField] private StartGameEvent OnClientsConnected;
        
        [SerializeField] private TMP_Text countPlayersText;

        private int currentCountPlayers;
        private int CurrentCountPlayers
        {
            set {
                currentCountPlayers = value;
                if (countPlayersText != null) {
                    Debug.Log(currentCountPlayers + " value: " + value);
                    countPlayersText.text = currentCountPlayers + "/" + NetworkLobbyData.Instance.targetCountPlayers;
                }
            }

            get => currentCountPlayers;
        }

        public override void OnNetworkSpawn() {
            if (!IsServer) return;
            NetworkManager.Singleton.OnClientConnectedCallback += HandlePlayerConnection;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandlePlayerDisconnection;
            UpdateVariableClientRpc(1);
        }

        private void OnDisable() {
            if (!IsServer || !NetworkManager.Singleton) return;
            NetworkManager.Singleton.OnClientConnectedCallback -= HandlePlayerConnection;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandlePlayerDisconnection;
        }

        private void Start() {
            StartCoroutine(HandleStartingGame());
        }

        private void HandlePlayerConnection(ulong id) {
            var countPlayers = CurrentCountPlayers + 1;
            UpdateVariableClientRpc(countPlayers);
        }

        private void HandlePlayerDisconnection(ulong id) {
            var countPlayers = CurrentCountPlayers - 1;
            UpdateVariableClientRpc(countPlayers);
        }

        [ClientRpc]
        private void UpdateVariableClientRpc(int currentLengthLobby) {
            CurrentCountPlayers = currentLengthLobby;
        }

        private IEnumerator HandleStartingGame() {
            if(!IsServer) yield break;
            while (true) {
                if (NetworkManager.Singleton != null) {
                    if (NetworkLobbyData.Instance.targetCountPlayers == NetworkManager.Singleton.ConnectedClients.Count) {
                        InvokeEventClientRpc();
                        yield break;
                    }
                }
                
                yield return new WaitForSeconds(0.1f);
            }
        }

        [ClientRpc]
        private void InvokeEventClientRpc() {
            OnClientsConnected?.Invoke();
        }

        public void StartingGame() {
            CanvasAnimSettings animSettings = new CanvasAnimSettings();
            animSettings.turnOfAnim = true;

            if (!IsServer) {
                CanvasAnimation.Instance.PlayAnim(CanvasAnimationType.CricleGrowing, animSettings);
            }
            else if (IsServer) {
                animSettings.animSceneLoading = new CanvasAnimSettings.AnimSceneLoading("Gameplay");
                animSettings.animSceneLoading.networkLoader = true;
                CanvasAnimation.Instance.PlayAnim(CanvasAnimationType.CricleGrowing, animSettings);
            }
        }
    }
}