
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PreparedGameInterface : MonoBehaviour
{
    [SerializeField] private CanvasGroup loader;
    [SerializeField] private TMP_Text startText;
    [SerializeField] private TMP_Text visualizeText;
    [SerializeField] [Range(1.1f,3.0f)] private float speedSmoothAlpha = 1.8f;
    [SerializeField] private int timePreparing;

    [SerializeField] private List<string> visualizeListTexts = new List<string>();

    [SerializeField] private float durationAlpha = 1f;

    [SerializeField] private float punchDuration = 1f;
    [SerializeField] private int punchVibrato = 2;
    [SerializeField] private float punchElasticity = 1f;
    
    public void InvokePreparedGame() {
        gameObject.SetActive(true);
        StartCoroutine(InvokePreparing(1));
    }

    private IEnumerator InvokePreparing(float timeToPrepaingScene) {
        yield return new WaitForSeconds(timeToPrepaingScene);
        yield return VisualizerInterfaceFighting();
        yield return TurnCanvasGroup(AlphaType.Close, false, false);
        yield return DownTime();
    }
    private IEnumerator VisualizerInterfaceFighting() {
        foreach (var text in visualizeListTexts) {
            visualizeText.text = text;
            visualizeText.transform.DOPunchScale(visualizeText.transform.localScale * 1.5f, punchDuration,punchVibrato,punchElasticity);
            yield return new WaitForSeconds(punchDuration);
        }
    }
    private IEnumerator DownTime() {
        startText.transform.DOScale(2f, 1f);
        for (int i = timePreparing; i >= 0; i--) {
             startText.text = "Starting in " + i;
            yield return new WaitForSeconds(1f);
        }
        
        gameObject.SetActive(false);
        GameManager.Instance.StartedGame?.Invoke();
    }
    
    private IEnumerator TurnCanvasGroup(AlphaType alphaType,bool interactable, bool blockRaycasts) {
        loader.interactable = interactable;
        loader.blocksRaycasts = blockRaycasts;

        float endValue = alphaType == AlphaType.Close ? 0 : 1;

        loader.DOFade(endValue, durationAlpha);

        yield return new WaitForSeconds(0.01f);
    }
    private enum AlphaType
    {
        Open,
        Close
    }
}
