using System.Collections;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Authentication : MonoBehaviour
{
    public Button loginInBT;
    public TMP_InputField nickNameIF;
    public TMP_Text errorMessage;

    private void Awake() {
        loginInBT.onClick.AddListener(LoginButton);
    }

    private async void LoginButton() {
        var nickName = nickNameIF.text;
        if (NetworkPersonData.Instance.NickNameRules(nickName)) {
            NetworkPersonData.Instance.NickName = nickName;

            await Authenticate(NetworkPersonData.Instance.NickName);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            return;
        }

        errorMessage.text = " Nickname must be between 5 and 12 characters long ";
        StartCoroutine(DisableErrorMessage(errorMessage,5f));
    }
    
    private IEnumerator DisableErrorMessage(TMP_Text message, float time) {
        yield return new WaitForSeconds(time);
        message.text = "";
    }
    
    
    private async Task Authenticate(string playerName) {
        InitializationOptions initializationOptions = new InitializationOptions();
        initializationOptions.SetProfile(playerName);

        await UnityServices.InitializeAsync(initializationOptions);

        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in! " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
}
