using DG.Tweening;
using UnityEngine;
using System;

namespace Gameplay
{
    public class Animator : MonoBehaviour
    {
        private GridController gridController;

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

            Sequence sequence = DOTween.Sequence();
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
    }
}