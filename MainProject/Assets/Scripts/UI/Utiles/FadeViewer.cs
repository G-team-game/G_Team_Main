using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
public class FadeViewer : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float initializeAlpha = 0;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup.alpha = initializeAlpha;
        canvasGroup.interactable = initializeAlpha == 1;
        canvasGroup.blocksRaycasts = initializeAlpha == 1;
    }

    public void FadeIn(UnityAction callBack, float dur = 0.3f, float delay = 0.0f)
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        DOTween.To(() => canvasGroup.alpha, (v) => canvasGroup.alpha = v, 1.0f, dur).OnComplete(() =>
        {
            if (callBack != null)
            {
                callBack();
            }

        }).SetDelay(delay).SetUpdate(true);
    }

    public void FadeOut(float dur = 0.3f, float delay = 0.0f)
    {
        DOTween.To(() => canvasGroup.alpha, (v) => canvasGroup.alpha = v, 0.0f, dur).OnComplete(() =>
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

        }).SetDelay(delay).SetUpdate(true);
    }
}
