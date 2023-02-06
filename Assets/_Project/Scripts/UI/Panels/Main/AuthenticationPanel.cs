using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticationPanel : MainPanel
{
    public Button loginInBT;
    public TMP_InputField nickNameIF;
    public TMP_Text errorMessage;

    protected override void Awake()
    {
        base.Awake();
        LobbyManager.Instance.OnAuthenticationSigned += () => { PanelActivity.Instance.MoveTo(MainPanels.MainMenuPanel); };
        
        loginInBT.onClick.AddListener(LoginButton);
    }

    private void LoginButton() {
        var nickName = nickNameIF.text;
        if (NetworkPersonData.Instance.NickNameRules(nickName)) {
            NetworkPersonData.Instance.NickName = nickName;
            LobbyManager.Instance.Authenticate(nickName);

            return;
        }

        errorMessage.text = " Nickname must be between 5 and 12 characters long ";
        StartCoroutine(DisableErrorMessage(errorMessage,5f));
    }
    protected override void Start()   
    {    
        base.Start();
        
        if (LobbyManager.Instance.IsSinged()) 
            LobbyManager.Instance.AuthenticateData(NetworkPersonData.Instance.NickName);
        else if(!LobbyManager.Instance.IsSinged() && !string.IsNullOrEmpty(NetworkPersonData.Instance.NickName)) 
            LobbyManager.Instance.Authenticate(NetworkPersonData.Instance.NickName);
        else
            PanelActivity.Instance.MoveTo(MainPanels.AuthenticationPanel);
    }

    private IEnumerator DisableErrorMessage(TMP_Text message, float time) {
        yield return new WaitForSeconds(time);
        message.text = "";
    }
    
    protected override void OnSelectionPanel()
    {
        
    }
    protected override void OnDeselectionPanel()
    {
        
    }
}
