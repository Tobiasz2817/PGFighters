using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ParrelSync;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LobbyManager : MonoBehaviour
{
    public enum GameMode {
        CaptureTheFlag = 1,
        Conquest = 2,
        Deathmatch = 3
    }
    
    public static LobbyManager Instance { get; private set; }

    public event Action OnLeftLobby;
    public event Action OnGameStarted;
    public event Action OnAuthenticationSigned;

    public event Action<Lobby> OnJoinedLobby;
    public event Action<Lobby> OnJoinedLobbyUpdate;
    public event Action<Lobby> OnKickedFromLobby;
    public event Action<Lobby> OnLobbyGameModeChanged;
    
    public event Action<List<Lobby>> OnLobbyListChanged;
    
    private float heartbeatTimer;
    private float lobbyPollTimer;
    private float refreshLobbyListTimer = 5f;
    private Lobby joinedLobby;
    private string playerName;

    private bool someMethodAreInvoking = false;
    private void Awake() {
        Instance = this;
    }

    private void Update() {
        HandleLobbyHeartbeat();
        HandleRefreshLobbyList();
        HandleLobbyPolling();
    }

    public bool SomeMethodAreInvoking() {
        return someMethodAreInvoking;
    }
    
    public async void Authenticate(string playerName) {
        this.playerName = playerName;
        InitializationOptions initializationOptions = new InitializationOptions();
        initializationOptions.SetProfile(playerName);

        await UnityServices.InitializeAsync(initializationOptions);

        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in! " + AuthenticationService.Instance.PlayerId);

            RefreshLobbyList();
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        
        OnAuthenticationSigned?.Invoke();
    }
    public async void AuthenticateData(string playerName) {
        this.playerName = playerName;
        
        OnAuthenticationSigned?.Invoke();
    }
    public bool IsSinged() {
        return UnityServices.State == ServicesInitializationState.Initialized ? AuthenticationService.Instance.IsSignedIn : false;
    }
    private void HandleRefreshLobbyList() {
        if (UnityServices.State == ServicesInitializationState.Initialized && AuthenticationService.Instance.IsSignedIn) {
            refreshLobbyListTimer -= Time.deltaTime;
            if (refreshLobbyListTimer < 0f) {
                float refreshLobbyListTimerMax = 5f;
                refreshLobbyListTimer = refreshLobbyListTimerMax;

                RefreshLobbyList();
            }
        }
    }

    private async void HandleLobbyHeartbeat() {
        if (IsLobbyHost()) {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0f) {
                float heartbeatTimerMax = 15f;
                heartbeatTimer = heartbeatTimerMax;

                Debug.Log("Heartbeat");
                await LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
    }

    private async void HandleLobbyPolling() {
        if (joinedLobby != null) {
            lobbyPollTimer -= Time.deltaTime;
            if (lobbyPollTimer < 0f) {
                float lobbyPollTimerMax = 1.1f;
                lobbyPollTimer = lobbyPollTimerMax;
                
                joinedLobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);

                OnJoinedLobbyUpdate?.Invoke(joinedLobby);
                
                if (!IsPlayerInLobby()) {
                    Debug.Log("Kicked from Lobby!");

                    OnKickedFromLobby?.Invoke(joinedLobby);

                    joinedLobby = null;
                }
                
                if (IsGameStarted()) {
                    if (!IsLobbyHost()) {
                        RelayManager.Instance.JoinRelay(joinedLobby.Data[LobbyConstans.KEY_START_GAME].Value);
                    }
 
                    joinedLobby = null;
                }
            }
        }
    }

    public Lobby GetJoinedLobby() {
        return joinedLobby;
    }

    public bool IsLobbyHost() {
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }
    
    private bool IsPlayerInLobby() {
        if (joinedLobby != null && joinedLobby.Players != null) 
            foreach (Player player in joinedLobby.Players) 
                if (player.Id == AuthenticationService.Instance.PlayerId)
                    return true;
        
        return false;
    }

    private bool IsGameStarted() {
        if (joinedLobby != null && joinedLobby.Players != null) 
            if (joinedLobby.Data[LobbyConstans.KEY_START_GAME].Value != "0")
                return true;

        return false;
    }
    private Player GetPlayer() {
        return new Player(AuthenticationService.Instance.PlayerId, null, new Dictionary<string, PlayerDataObject> {
            { LobbyConstans.KEY_PLAYER_NAME, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName) },
            { LobbyConstans.KEY_PLAYER_ACTIVE, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, false.ToString()) }
        });
    }

    public void ChangeGameMode() {
        if (IsLobbyHost()) {
            GameMode gameMode =
                Enum.Parse<GameMode>(joinedLobby.Data[LobbyConstans.KEY_GAME_MODE].Value);

            switch (gameMode) {
                default:
                case GameMode.CaptureTheFlag:
                    gameMode = GameMode.Conquest;
                    break;
                case GameMode.Conquest:
                    gameMode = GameMode.CaptureTheFlag;
                    break;
            }

            UpdateLobbyGameMode(gameMode);
        }
    }

    public async void CreateLobby(string lobbyName, int maxPlayers, bool isPrivate, GameMode gameMode) {
        Player player = GetPlayer();

        CreateLobbyOptions options = new CreateLobbyOptions {
            Player = player,
            IsPrivate = isPrivate,
            Data = new Dictionary<string, DataObject> {
                { LobbyConstans.KEY_GAME_MODE, new DataObject(DataObject.VisibilityOptions.Public, gameMode.ToString()) },
                { LobbyConstans.KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, "0") }
            }
        };

        joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);

        UpdatePlayerActive(true);
        
        OnJoinedLobby?.Invoke(joinedLobby);
    }

    public async void StartGame() {
        if (IsLobbyHost()) {
            try {
                string relayCode = await RelayManager.Instance.CreateRelay(joinedLobby.MaxPlayers);

                joinedLobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions() {
                    Data = new Dictionary<string, DataObject> {
                        { LobbyConstans.KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member,relayCode) }
                    },
                    IsLocked = true
                });

                StartCoroutine(HandleStartingGame(joinedLobby.Players.Count));
                Debug.Log(" Waiting for players.... ");
            }
            catch (LobbyServiceException e){
                Console.WriteLine(e);
            }
        }
    }
    private IEnumerator HandleStartingGame(int countPlayers) {
        while (true) {
            yield return new WaitForSeconds(1f);
            if (countPlayers == NetworkManager.Singleton.ConnectedClients.Count) {
                OnGameStarted?.Invoke();
                StopAllCoroutines();
                break;
            }
        }
    }

    public async void RefreshLobbyList() {
        try {
            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 25;
            
            options.Filters = new List<QueryFilter> {
                new QueryFilter(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
                    value: "0")
            };
            
            options.Order = new List<QueryOrder> {
                new QueryOrder(
                    asc: false,
                    field: QueryOrder.FieldOptions.Created)
            };

            QueryResponse lobbyListQueryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            OnLobbyListChanged?.Invoke(lobbyListQueryResponse.Results);
        } catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }

    public async void JoinLobby(string lobbyCode) {
        Player player = GetPlayer();

        joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode, new JoinLobbyByCodeOptions {
            Player = player
        });

        OnJoinedLobby?.Invoke(joinedLobby);
    }
    public async void QuickJoinLobby() {
        Player player = GetPlayer();

        joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync(new QuickJoinLobbyOptions() {
            Player = player
        });

        OnJoinedLobby?.Invoke(joinedLobby);
    }
    public async void JoinLobby(Lobby lobby) {
        Player player = GetPlayer();

        joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id, new JoinLobbyByIdOptions() {
            Player = player
        });

        OnJoinedLobby?.Invoke(joinedLobby);
    }
    public async void UpdatePlayerActive(bool isActive) {
        if (joinedLobby != null) {
            try {
                UpdatePlayerOptions options = new UpdatePlayerOptions();

                options.Data = new Dictionary<string, PlayerDataObject>() {
                    {
                        LobbyConstans.KEY_PLAYER_ACTIVE, new PlayerDataObject(
                            visibility: PlayerDataObject.VisibilityOptions.Public,
                            value: isActive.ToString())
                    }
                };

                string playerId = AuthenticationService.Instance.PlayerId;

                joinedLobby = await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, playerId, options);

                OnJoinedLobbyUpdate?.Invoke(joinedLobby);
            } catch (LobbyServiceException e) {
                Debug.Log(e);
            }
        }
    }
    
    public async void UpdateLobbyGameMode(GameMode gameMode) {
        try {
            Debug.Log("UpdateLobbyGameMode " + gameMode);
            
            joinedLobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions {
                Data = new Dictionary<string, DataObject> {
                    { LobbyConstans.KEY_GAME_MODE, new DataObject(DataObject.VisibilityOptions.Public, gameMode.ToString()) }
                }
            });

            OnLobbyGameModeChanged?.Invoke(joinedLobby);
        } catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }
    
    public bool PlayersReady()
    {
        if (joinedLobby == null) return false;
            
        foreach (var player in joinedLobby.Players)
            if (!bool.Parse(player.Data[LobbyConstans.KEY_PLAYER_ACTIVE].Value))
                return false;

        return true;
    }
    
    public async void LeaveLobby() {
        if (joinedLobby != null) {
            try {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);

                joinedLobby = null;

                OnLeftLobby?.Invoke();
            } catch (LobbyServiceException e) {
                Debug.Log(e);
            }
        }
    }

    public async void KickPlayer(string playerId) {
        if (IsLobbyHost()) {
            try {
                Debug.Log("Drop player");
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
            } catch (LobbyServiceException e) {
                Debug.Log(e);
            }
        }
    }

}
public class LobbyConstans {
    public const string KEY_PLAYER_NAME = "PlayerName";
    public const string KEY_PLAYER_ACTIVE = "Active";
    public const string KEY_GAME_MODE = "GameMode";
    public const string KEY_START_GAME = "StartGame";
}
