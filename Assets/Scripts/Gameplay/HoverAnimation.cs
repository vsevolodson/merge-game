using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverAnimation :
    MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float animationDuration = 0.15f;

    private Tween currentTween;

    public void OnPointerEnter(PointerEventData eventData)
    {
        currentTween?.Kill();

        currentTween = transform.DOScale(
            Vector3.one * hoverScale,
            animationDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        currentTween?.Kill();

        currentTween = transform.DOScale(
            Vector3.one,
            animationDuration);
    }

    private void OnDestroy()
    {
        currentTween?.Kill();
    }
}