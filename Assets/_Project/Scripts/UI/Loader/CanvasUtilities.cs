using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class CanvasUtilities : MonoBehaviour
{
    public static CanvasUtilities Instance { get; private set; }
    
    [SerializeField] [Range(1.1f,3.0f)]
    private float speedSmoothAlpha = 1.8f;

    private void Awake() {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    private void CreateInstances() {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(this);
    }
    
    
    [SerializeField] private TMP_Text utilitiesText;
    [SerializeField] private TMP_Text errorMessage;
    [SerializeField] private CanvasGroup loader;
   
    public async Task EnableUtilities(UtilitiesParameters utilitiesParameters) {
        this.utilitiesText.text = utilitiesParameters.displayMessage;
        
        await TurnCanvasGroup(AlphaType.Open,true,true);
    }

    public void DisplayErrorMessage(string errorMessage) {
        this.errorMessage.text = errorMessage;
    }

    public async Task DisableUtilities() {
        await TurnCanvasGroup(AlphaType.Close,false,false);
        this.utilitiesText.text = "Loading ...";
        this.errorMessage.text = " ";
    }

    private async Task SmoothAlpha(AlphaType alphaType) {
        float timer = 0;

        int a = alphaType == AlphaType.Open ? 0 : 1;
        int b = alphaType == AlphaType.Close ? 0 : 1;
        
        while (timer < 1)
        {
            timer += (Time.deltaTime * speedSmoothAlpha);
            float alpha = Mathf.Lerp(a, b, timer);

            loader.alpha = alpha;
            await Task.Delay(1);
        }
    }
    private async Task TurnCanvasGroup(AlphaType alphaType,bool interactable, bool blockRaycasts) {
        loader.interactable = interactable;
        loader.blocksRaycasts = blockRaycasts;
        await SmoothAlpha(alphaType);
    }

    public Task IsDone() {
        CreateInstances();
        return Task.Delay(1);
    }
}
public class UtilitiesParameters
{
    public string displayMessage;
    public string errorMessage;

    public UtilitiesParameters(string displayMessage, string errorMessage) {
        this.displayMessage = displayMessage;
        this.errorMessage = errorMessage;
    }
}

public enum AlphaType
{
    Open,
    Close
}