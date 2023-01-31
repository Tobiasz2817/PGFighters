using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbiesPanel : Panel
{
    [SerializeField] private TMP_InputField enterCodeIF;
    [SerializeField] private Button enterBT;
    [SerializeField] private Button enterQuickBT;

    [SerializeField] private Transform lobbyContent;
    [SerializeField] private Transform lobbyPrefab;

    protected override void Awake() {
        base.Awake();
        
        enterBT.onClick.AddListener(() => {
            LobbyManager.Instance.JoinLobby(enterCodeIF.text);
            PanelActivity.Instance.MoveTo(Panels.WaitingPanel);
        });
        
        enterQuickBT.onClick.AddListener(() => {
            LobbyManager.Instance.QuickJoinLobby();
            PanelActivity.Instance.MoveTo(Panels.WaitingPanel);
        });
    }

    protected override void Start() {    
        base.Start();

        LobbyManager.Instance.OnLobbyListChanged += UpdateLobbyList;
    }
    protected override void OnSelectionPanel() {
        base.OnSelectionPanel();
    } 
    protected override void OnDeselectionPanel() {
        base.OnDeselectionPanel();
    }
    private void UpdateLobbyList(List<Lobby> lobbies) {
        ClearLobby();
        
        foreach (Lobby lobby in lobbies) {
            Transform playerSingleTransform = Instantiate(lobbyPrefab, lobbyContent);
            playerSingleTransform.gameObject.SetActive(true);
            LobbyListUI lobbyListUI = playerSingleTransform.GetComponent<LobbyListUI>();

            lobbyListUI.UpdateLobby(lobby);
        }
    }

    private void ClearLobby() {
        foreach (Transform child in lobbyContent) {
            if (child == lobbyPrefab) continue;
            Destroy(child.gameObject);
        }
    }
    
}