using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticationPanel : MainPanel
{
    public Button loginInBT;
    public TMP_InputField nickNameIF;

    protected override void Awake()
    {
        base.Awake();
        LobbyManager.Instance.OnAuthenticationSigned += () => { PanelActivity.Instance.MoveTo(MainPanels.MainMenuPanel); };
        
        loginInBT.onClick.AddListener(() => {
            LobbyManager.Instance.Authenticate(nickNameIF.text);
        });
    }
    protected override void Start()   
    {    
        base.Start();
        PanelActivity.Instance.MoveTo(MainPanels.AuthenticationPanel);
    }
    protected override void OnSelectionPanel()
    {
        base.OnSelectionPanel();
    }
    protected override void OnDeselectionPanel()
    {
        base.OnDeselectionPanel();
    }
}
