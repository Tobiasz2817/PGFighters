using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrepareQuitGameInterface : MonoBehaviour {
    [SerializeField] private TMP_Text leavingText;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private int timePreparing;

    [SerializeField] private float punchDuration = 1f;
    [SerializeField] private float sizeScale = 5f;

    public static event Action OnGameOverInterfaceFinish;
    public void InvokePreparedGame(string finishedText) {
        gameObject.SetActive(true);
        StartCoroutine(InvokePreparingGameOver(1, finishedText));
    }

    private IEnumerator InvokePreparingGameOver(float timeToPrepaingScene, string finishedText) {
        yield return new WaitForSeconds(timeToPrepaingScene);
        yield return VisualizerQuitInterface(finishedText);
        //yield return DownTime();
        
        OnGameOverInterfaceFinish?.Invoke();
    }

    private IEnumerator VisualizerQuitInterface(string finishedText) {
        gameOverText.text = finishedText;
        gameOverText.transform.DOScale(gameOverText.transform.localScale * sizeScale, punchDuration);
        yield return new WaitForSeconds(punchDuration);
    }

    private IEnumerator DownTime() {
        for (int i = timePreparing; i >= 0; i--) {
            leavingText.text = "Leaving " + i;

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(timePreparing);
        gameObject.SetActive(false);
    }
}