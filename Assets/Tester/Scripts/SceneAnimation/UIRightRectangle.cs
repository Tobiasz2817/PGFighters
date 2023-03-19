using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace SceneAnimation
{
    public class UIRightRectangle : UIAnimBased
    {
        public override IEnumerator AnimateOn() {
            ActiveSelf(true);
            ActiveCanvasGroup(false, false);
            Debug.Log(moveTo.x);
            yield return sprite.transform.DOLocalMoveX(moveTo.x, 2f);
            yield return new WaitUntil(() => sprite.transform.localPosition.x == moveTo.x);
        }

        public override IEnumerator AnimateOf() {
            Debug.Log("Anim of");
            yield return sprite.transform.DOLocalMoveX((startMoveValue.x * -1), 2f);
            yield return new WaitUntil(() => sprite.transform.localPosition.x == (startMoveValue.x * -1));
            ActiveCanvasGroup(true, true);
            ActiveSelf(false);
        }
    }
}


