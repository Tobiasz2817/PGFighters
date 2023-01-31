
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyListUI : MonoBehaviour
{
    [SerializeField] private TMP_Text roomNameText;
    [SerializeField] private Button joinLobbyBT;

    private Lobby lobby;
    
    private void Awake() {
        joinLobbyBT.onClick.AddListener(() => {
            if (lobby != null) {
                LobbyManager.Instance.JoinLobby(lobby);
                PanelActivity.Instance.MoveTo(Panels.WaitingPanel);
            }
        });
    }
    
    public void UpdateLobby(Lobby lobby) {
        this.lobby = lobby;

        roomNameText.text = lobby.Name;
    }
}
