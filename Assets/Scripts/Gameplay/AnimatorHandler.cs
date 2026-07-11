using DG.Tweening;
using UnityEngine;
using System;

public class AnimatorHandler : MonoBehaviour
{
    private GridController gridController;
    private Sequence sequence;

    private const float AnimationDuration = 0.1f;
    private const float MergeScaleMultiplier = 1.5f;

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
                AnimationDuration
            )
        );
        sequence.Append(
            movingItem.transform.DOScale(Vector3.zero, AnimationDuration)
        );
        sequence.AppendCallback(() =>
        {
            onMerge?.Invoke();
        });
        sequence.Append(
            targetItem.transform.DOScale(Vector3.one * MergeScaleMultiplier, AnimationDuration)
        );
        sequence.Append(
            targetItem.transform.DOScale(Vector3.one, AnimationDuration)
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