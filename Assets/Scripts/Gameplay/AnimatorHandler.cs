using DG.Tweening;
using UnityEngine;
using System;

public class AnimatorHandler : MonoBehaviour
{
    private Sequence sequence;
    private CanvasGroup group;

    private const float AnimationDuration = 0.1f;
    private const float MergeScaleMultiplier = 1.5f;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }

    public void PlayMerge(
        ItemView movingItem,
        ItemView targetItem,
        Action onMerge,
        Action onComplete)
    {
        BeginAnimation();

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
            EndAnimation();
            onComplete?.Invoke();
        });
    }

    public void BeginAnimation()
    {        
        group.interactable = false;
        group.blocksRaycasts = false;
    }

    public void EndAnimation()
    {
        group.interactable = true;
        group.blocksRaycasts = true;
    }

    private void OnDestroy()
    {
        EndAnimation();

        sequence?.Kill();
    }
}