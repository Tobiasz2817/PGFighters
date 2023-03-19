using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace SceneAnimation
{
    public class TextCountingDown : NetworkBehaviour
    {
        [Serializable] public class CountDownEvent : UnityEvent {}
        [SerializeField] private CountDownEvent countDownEvent;
        
        [SerializeField] private int timeCounting = 3;
        [SerializeField] private TMP_Text textCounting;
        [SerializeField] private CanvasGroup canvasGroup;
        
        
        public void InvokeCounting() {
            if (!IsServer) return;
            InvokeCountingDownClientRpc();
        }
        
        [ClientRpc]
        private void InvokeCountingDownClientRpc() {
            StartCoroutine(DownTime());
        }
        
        private IEnumerator DownTime() {
            yield return canvasGroup.DOFade(1, 2f);
            
            for (int i = timeCounting; i >= 0; i--) {
                textCounting.text = "Starting in " + i;
                yield return new WaitForSeconds(1f);
            }
            countDownEvent?.Invoke();
        }
    }
}