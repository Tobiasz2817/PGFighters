using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomPanel : Panel
{
    [SerializeField] private Button createRoomBT; 
    [SerializeField] private Button quickCreateRoomBT; 
    [SerializeField] private Button gameModeButton;
    
    [SerializeField] private TMP_InputField roomNameIF; 
    [SerializeField] private Toggle isPrivateTG;
    [SerializeField] private TextMeshProUGUI gameModeText;
    
    private LobbyManager.GameMode gameMode = LobbyManager.GameMode.CaptureTheFlag;
    protected override void Awake()
    {
        base.Awake();
        
        UpdateText();
        
        createRoomBT.onClick.AddListener(() => {
            LobbyManager.Instance.CreateLobby(roomNameIF.text,2,isPrivateTG.isOn,gameMode);
            PanelActivity.Instance.MoveTo(Panels.WaitingPanel);
        });
        
        quickCreateRoomBT.onClick.AddListener(() => {
            LobbyManager.Instance.CreateLobby("Room " + Random.Range(10,1000000),2,false,gameMode);
            PanelActivity.Instance.MoveTo(Panels.WaitingPanel);
        });
        
        gameModeButton.onClick.AddListener(() => {
            gameMode++;
            if (int.TryParse(gameMode.ToString(), out _))
            {
                gameMode = (LobbyManager.GameMode)(1);
            }
            
            UpdateText();
        }); 
    }
    protected override void Start()
    {    
        base.Start();
    }
    protected override void OnSelectionPanel()
    {
        base.OnSelectionPanel();
    }
    protected override void OnDeselectionPanel()
    {
        base.OnDeselectionPanel();
    }
    
    private void UpdateText() {
        gameModeText.text = gameMode.ToString();
    }

    
}
