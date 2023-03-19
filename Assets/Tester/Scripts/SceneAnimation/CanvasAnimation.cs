using Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneAnimation
{
    public class CanvasAnimation : SingletonPersistent<CanvasAnimation>
    {
        #region Get Dict From Editor
        [Serializable]
        public class DictionaryDisplayer
        {
            public CanvasAnimationType key;
            public UIAnimBased value;
        }
        [SerializeField] private List<DictionaryDisplayer> playAnimations = new List<DictionaryDisplayer>();
        #endregion
        
        public Action OnAnimFinish;

        private Coroutine coroutine;

        private readonly Dictionary<CanvasAnimationType, UIAnimBased> animations = new Dictionary<CanvasAnimationType, UIAnimBased>();

        public override void Awake() {
            base.Awake();

            foreach (var dict in playAnimations) {
                animations.Add(dict.key,dict.value);
            }
        }

        public void PlayAnim(CanvasAnimationType type, CanvasAnimSettings canvasAnimSettings,params Func<Task>[] actions) {
            if (coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(InvokeAnim(type,canvasAnimSettings,actions));
        }
        
        private IEnumerator InvokeAnim(CanvasAnimationType type, CanvasAnimSettings canvasAnimSettings, params Func<Task>[] actions) {
            yield return animations[type].AnimateOn();
            OnAnimFinish?.Invoke();

            if(actions != null)
                foreach (var action in actions)
                    yield return Wait(action);
            
            yield return new WaitForSeconds(canvasAnimSettings.durationBetweenLoading);

            if (canvasAnimSettings.animSceneLoading != null) yield return canvasAnimSettings.animSceneLoading.LoadAsync();
            if (canvasAnimSettings.turnOfAnim) yield return animations[type].AnimateOf();
        }
        
        private IEnumerator Wait(Func<Task> action) {
            var task = action?.Invoke();
            while (!task.IsCompleted)
            {
                yield return null;
            }
            
            Debug.Log("Asynchroniczna operacja została zakończona.");
        }
    }
    public enum CanvasAnimationType
    {
        CricleGrowing,
        RightRectangle,
        SpreadingCones,
    }

    public class CanvasAnimSettings
    {
        public bool turnOfAnim = false;
        public float durationBetweenLoading = 0f;
        public AnimSceneLoading animSceneLoading;
        
        public CanvasAnimSettings() : this(true,0f,null) {
        }
        public CanvasAnimSettings(bool turnOfAnim_,float durationBetweenLoading_,AnimSceneLoading animSceneLoading_) {
            this.turnOfAnim = turnOfAnim_;
            this.durationBetweenLoading = durationBetweenLoading_;
            this.animSceneLoading = animSceneLoading_;
        }

        public class AnimSceneLoading
        {
            public int indexScene = -1;
            public string nameScene = string.Empty;
            public bool networkLoader = false;
            public Action OnSceneLoaded;
            
            public AnimSceneLoading(int indexScene_) {
                this.indexScene = indexScene_;
            }

            public AnimSceneLoading(string nameScene_) {
                this.nameScene = nameScene_;
            }
            
            public IEnumerator LoadAsync() {
                AsyncOperation asyncLoad = null;

                if (!networkLoader) {
                    if (indexScene != -1) 
                        asyncLoad = SceneManager.LoadSceneAsync(indexScene);
                    else if (!string.IsNullOrEmpty(nameScene))
                        asyncLoad = SceneManager.LoadSceneAsync(nameScene);
                    else
                        yield break;
                }
                else {
                    if (!string.IsNullOrEmpty(nameScene))
                        if(NetworkManager.Singleton != null)
                        {
                            if (NetworkManager.Singleton.SceneManager != null) {
                                NetworkManager.Singleton.SceneManager.LoadScene(nameScene, LoadSceneMode.Single);
                            }
                        }

                    yield break;
                }

                while (asyncLoad != null && !asyncLoad.isDone)
                {
                    yield return null;
                }
                
                Debug.Log("INVOKE EVENT");
                OnSceneLoaded?.Invoke();
            }
        }
    }
}