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
    [SerializeField] private int lengthLoadingDot = 3;
    
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private int timePreparing;
    [SerializeField] private float punchDuration = 1f;
    [SerializeField] private float sizeScale = 5f;

    [SerializeField] private GameObject dot;
    [SerializeField] private float dotResize = 40f;
    [SerializeField] private float dotDuration = 2f;
    
    private bool isLoading = true;

    public static event Action OnGameOverInterfaceFinish;
    public void InvokePreparedGame(string finishedText) {
        gameObject.SetActive(true);
        StartCoroutine(InvokePreparingGameOver(finishedText));
    }

    private IEnumerator InvokePreparingGameOver(string finishedText) {

        StartCoroutine(LeaveLoading());
        DotResize();
        yield return VisualizerQuitInterface(finishedText);
        yield return new WaitForSeconds(timePreparing);
        isLoading = true;

        OnGameOverInterfaceFinish?.Invoke();
    }

    private IEnumerator VisualizerQuitInterface(string finishedText) {
        gameOverText.text = finishedText;
        gameOverText.transform.DOScale(gameOverText.transform.localScale * sizeScale, punchDuration);

        yield return new WaitForSeconds(punchDuration);
    }

    private IEnumerator LeaveLoading() {
        while (isLoading) {
            string x = ".";
            for (int i = lengthLoadingDot; i > 0; i--) {
                leavingText.text = "Loading " + x;
                yield return new WaitForSeconds(0.75f);
                x += " .";
            }
        }
    }
    
    private void DotResize() {
        dot.transform.DOScale(dotResize, dotDuration);
    }
}