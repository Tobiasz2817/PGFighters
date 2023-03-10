using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ConnectionManager : MonoBehaviour
{
    [SerializeField] private Button joinButton;
    [SerializeField] private Button startButton;
    [SerializeField] private CanvasGroup canvasGroup;

    private Lobby currentLobby;
    private void Awake() {
        joinButton.onClick.AddListener(JoinButton);
        startButton.onClick.AddListener(StartButton);
    }

    async void Start() 
    {
        await Auth();
        StartCoroutine(ControlButtonsState(true,2f));
    }


    private async Task Auth() {
        string nickName = "Player " + Random.Range(1, 1000000);
        var options = new InitializationOptions();
        options.SetProfile(nickName);

        await UnityServices.InitializeAsync(options);
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private async void StartButton() {
        var roomName = "Room " + Random.Range(1, 100000);
        var player = GetPlayer();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
        string relayCode = await CreateRelay(8);

        var lobbyOptions = new CreateLobbyOptions();
        lobbyOptions.Player = player;
        lobbyOptions.Data = new Dictionary<string, DataObject>(){ {LobbyDependencies.ENTER_KEY, new DataObject(DataObject.VisibilityOptions.Public, relayCode)} };
        
        currentLobby = await LobbyService.Instance.CreateLobbyAsync(roomName, 8, lobbyOptions);
    }

    private Player GetPlayer() {
        return new Player(AuthenticationService.Instance.PlayerId);
    }

    private async void JoinButton() {
        Player player = GetPlayer();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
        currentLobby = await LobbyService.Instance.QuickJoinLobbyAsync(new QuickJoinLobbyOptions() {
            Player = player
        });
        

        if (currentLobby.Data[LobbyDependencies.ENTER_KEY].Value != "0") {
            await JoinRelay(currentLobby.Data[LobbyDependencies.ENTER_KEY].Value);
        }
        else {
            Debug.Log("Enter Key if wrong");
        }
    }

    private IEnumerator ControlButtonsState(bool state,float duration) {
        canvasGroup.blocksRaycasts = state;
        canvasGroup.interactable = state;

        var alpha = state ? 1f : 0f;

        yield return canvasGroup.DOFade(alpha,duration);
    }
    
    private async Task<string> CreateRelay(int playersCount) {
        try {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(playersCount);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData);
            NetworkManager.Singleton.StartHost();

            return joinCode;
        }
        catch (RelayServiceException e) {
            Debug.Log(e);
            return null;
        }
    }
    
    private async Task JoinRelay(string joinCode) {
        try {
            var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            Debug.Log("START JOIN CONNECT");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData, allocation.HostConnectionData);
            NetworkManager.Singleton.StartClient();
            Debug.Log("Start as client");
        }
        catch (RelayServiceException e) {
            Debug.Log(e);
        }
    }
}

public struct LobbyDependencies
{
    public static string ENTER_KEY = "ENTER_KEY";

}
