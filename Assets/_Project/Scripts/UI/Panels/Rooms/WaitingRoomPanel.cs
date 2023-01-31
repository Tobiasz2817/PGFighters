using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaitingRoomPanel : Panel
{
    [SerializeField] private Transform playerTemplate;
    [SerializeField] private Transform container;
    
    [SerializeField] private Button changeGameModeButton;
    [SerializeField] private Button isReadyBT;
    
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI playerCountText;
    [SerializeField] private TextMeshProUGUI gameModeText;
    [SerializeField] private TextMeshProUGUI enterCodeText;
    [SerializeField] private TextMeshProUGUI isReadyText;

    private bool isActive = false;
    protected override void Awake() {
        base.Awake();

        changeGameModeButton.onClick.AddListener(() => {
            LobbyManager.Instance.ChangeGameMode();
        });
        
        isReadyBT.onClick.AddListener(() => {
            if (LobbyManager.Instance.IsLobbyHost()) {
                LobbyManager.Instance.StartGame();
            }
            else {
                isActive = !isActive;
                LobbyManager.Instance.UpdatePlayerActive(isActive);
            }
        });
    }

    protected override void Start() {
        base.Start();
        
        LobbyManager.Instance.OnJoinedLobby += OnUpdateLobby;
        LobbyManager.Instance.OnJoinedLobbyUpdate += OnUpdateLobby;
        LobbyManager.Instance.OnLobbyGameModeChanged += OnUpdateLobby;
        LobbyManager.Instance.OnLeftLobby += OnLeftLobby;
        LobbyManager.Instance.OnKickedFromLobby += OnLeftLobby;
        LobbyManager.Instance.OnGameStarted += StartingGame;
    }

    private void StartingGame() {
        Debug.Log("Game is starting");

        NetworkManager.Singleton.SceneManager.LoadScene("Gameplay", LoadSceneMode.Single);
    }


    protected override void OnSelectionPanel() {
        base.OnSelectionPanel();
    }
    protected override void OnDeselectionPanel() {
        base.OnDeselectionPanel();
        LobbyManager.Instance.LeaveLobby();
    }
    private void OnLeftLobby(Lobby lobby) {
        OnLeftLobby();
    }
    private void OnLeftLobby() {
        ClearLobby();
        ResetLobbyInterface();
        PanelActivity.Instance.MoveTo(Panels.MainPanel);
    }
    
    private void OnUpdateLobby(Lobby lobby) {
        UpdateLobby();
    }
    private void UpdateLobby() {
        UpdateLobby(LobbyManager.Instance.GetJoinedLobby());
    }

    private void UpdateLobby(Lobby lobby) {
        ClearLobby();

        foreach (Player player in lobby.Players) {
            Transform playerSingleTransform = Instantiate(playerTemplate, container);
            playerSingleTransform.gameObject.SetActive(true);
            LobbyPlayerUI lobbyPlayerUI = playerSingleTransform.GetComponent<LobbyPlayerUI>();

            lobbyPlayerUI.SetKickPlayerButtonVisible(
                LobbyManager.Instance.IsLobbyHost() &&
                player.Id != AuthenticationService.Instance.PlayerId // Don't allow kick self
            );

            lobbyPlayerUI.UpdatePlayer(player);
        }

        var isHost = LobbyManager.Instance.IsLobbyHost();
        changeGameModeButton.gameObject.SetActive(isHost);

        if (isHost) {
            isReadyBT.gameObject.SetActive(LobbyManager.Instance.PlayersReady());
            SetTextActive("Start");
        }
        else SetTextActive(isActive ? "UnReady" : "Ready");
        
        lobbyNameText.text = lobby.Name;
        enterCodeText.text = lobby.LobbyCode;
        playerCountText.text = lobby.Players.Count + "/" + lobby.MaxPlayers;
        gameModeText.text = lobby.Data[LobbyConstans.KEY_GAME_MODE].Value;
    }

    private void ClearLobby() {
        foreach (Transform child in container) {
            if (child == playerTemplate) continue;
            Destroy(child.gameObject);
        }
    }
    
    private void ResetLobbyInterface() {
        isActive = false;
        SetTextActive(isActive ? "UnReady" : "Ready");
    }

    private void SetTextActive(string readyText) {
        isReadyText.text = readyText;
    }

}
