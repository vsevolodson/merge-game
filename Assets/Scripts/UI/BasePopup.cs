using DG.Tweening;
using UnityEngine;

public class BasePopup : MonoBehaviour
{
    [Header("Animation")]

    [SerializeField] private float showDuration = 0.25f;
    [SerializeField] private float hideDuration = 0.20f;

    [SerializeField] private float hiddenScale = 0.8f;
    [SerializeField] private float visibleScale = 1f;

    [SerializeField] private float hiddenAlpha = 0f;
    [SerializeField] private float visibleAlpha = 1f;

    [Header("References")]

    [SerializeField] protected CanvasGroup canvasGroup;
    [SerializeField] protected RectTransform window;
    [SerializeField] protected PopupManager popupManager;

    protected bool isOpened;

    public virtual void Show()
    {
        if (isOpened)
        {
            return;
        }

        isOpened = true;

        gameObject.SetActive(true);

        canvasGroup.alpha = hiddenAlpha;
        window.localScale = Vector3.one * hiddenScale;

        canvasGroup.DOFade(visibleAlpha, showDuration);

        window.DOScale(visibleScale, showDuration)
            .SetEase(Ease.OutBack);
    }

    public virtual void Hide()
    {
        if (!isOpened)
        {
            return;
        }

        isOpened = false;

        canvasGroup.DOFade(hiddenAlpha, hideDuration);

        window.DOScale(hiddenScale, hideDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }
}