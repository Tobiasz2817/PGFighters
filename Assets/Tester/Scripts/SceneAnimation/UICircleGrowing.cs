using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace SceneAnimation
{
    public class UICircleGrowing : UIAnimBased
    {
        public override IEnumerator AnimateOn() {
            ActiveCanvasGroup(false, false);
            ActiveSelf(true);
            yield return sprite.transform.DOScale(scaleTo, smoothDuration);
            yield return new WaitUntil(() => sprite.transform.localScale == scaleTo);
        }
        public override IEnumerator AnimateOf() {
            yield return sprite.transform.DOScale(startScaleValue, smoothDuration);
            yield return new WaitUntil(() => sprite.transform.localScale == startScaleValue);
            ActiveSelf(false);
            ActiveCanvasGroup(true, true);
        }
    }
}


