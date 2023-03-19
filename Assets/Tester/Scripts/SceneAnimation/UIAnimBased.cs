using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SceneAnimation
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIAnimBased : MonoBehaviour
    {
        [field: SerializeField] public CanvasAnimationType type { private set; get; }
    
        [SerializeField] protected CanvasGroup canvasGroup;
        [SerializeField] protected Image sprite;
    
        [SerializeField] protected Vector3 scaleTo;
        [SerializeField] protected Quaternion rotateTo;
        [SerializeField] protected Vector3 moveTo;
    
        [SerializeField] protected float smoothDuration = 2f;

        protected Vector3 startScaleValue;
        protected Quaternion startRotateValue;
        protected Vector3 startMoveValue;

        private void Start() {
            startScaleValue = sprite.transform.localScale;
            startRotateValue = sprite.transform.localRotation;
            startMoveValue = sprite.transform.localPosition;
        }

        public abstract IEnumerator AnimateOn();
        public abstract IEnumerator AnimateOf();

        protected void ActiveCanvasGroup(bool blocksRaycasts,bool interactable) {
            canvasGroup.blocksRaycasts = blocksRaycasts;
            canvasGroup.interactable = interactable;
        }

        protected void ActiveSelf(bool active) {
            gameObject.SetActive(active);
        }
    }
}