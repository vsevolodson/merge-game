using DG.Tweening;
using UnityEngine;
using System;

public class AnimatorHandler : MonoBehaviour
{
    private GridController gridController;
    private Sequence sequence;

    private void Awake()
    {
        gridController = GetComponent<GridController>();
    }

    public void PlayMerge(
        ItemView movingItem,
        ItemView targetItem,
        Action onMerge,
        Action onComplete)
    {
        gridController.BeginAnimation();

        sequence = DOTween.Sequence();
        sequence.Append(
            movingItem.transform.DOMove(
                targetItem.transform.position,
                0.1f
            )
        );
        sequence.Append(
            movingItem.transform.DOScale(Vector3.zero, 0.1f)
        );
        sequence.AppendCallback(() =>
        {
            onMerge?.Invoke();
        });
        sequence.Append(
            targetItem.transform.DOScale(Vector3.one * 1.5f, 0.1f)
        );
        sequence.Append(
            targetItem.transform.DOScale(Vector3.one, 0.1f)
        );
        sequence.OnComplete(() =>
        {
            gridController.EndAnimation();
            onComplete?.Invoke();
        });
    }

    private void OnDestroy()
    {
        gridController?.EndAnimation();

        sequence?.Kill();
    }
}