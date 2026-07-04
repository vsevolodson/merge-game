using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using DG.Tweening;

namespace Gameplay
{
    public class DragHandler :
        MonoBehaviour,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler
    {
        private ItemView itemView;

        private GridCell originalCell;
        private RectTransform canvasRect;

        private Vector2 localPoint;
        private Vector2 dragOffset;
        private GridController gridController;
        private Sequence sequence;
        private Animator animator;

        private readonly MergeHandler mergeHandler = new();

        public void Initialize(RectTransform canvasRect, GridController gridController, Animator animator)
        {
            this.canvasRect = canvasRect;
            this.gridController = gridController;
            this.animator = animator;
        }

        private void Awake()
        {
            itemView = GetComponent<ItemView>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (gridController.IsAnimating)
            {
                return;
            }

            originalCell.ClearItem();
            transform.SetParent(canvasRect);
            transform.SetAsLastSibling();

            localPoint = GetLocalPointerPosition(eventData);
            dragOffset = ((RectTransform)transform).anchoredPosition - localPoint;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (gridController.IsAnimating)
            {
                return;
            }

            localPoint = GetLocalPointerPosition(eventData);
            ((RectTransform)transform).anchoredPosition = localPoint + dragOffset;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (gridController.IsAnimating)
            {
                return;
            }

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            GridCell targetCell = null;
            foreach (var result in results)
            {
                targetCell = result.gameObject.GetComponent<GridCell>();

                if (targetCell != null) break;
            }

            if (targetCell != null && !targetCell.IsOccupied)
            {
                PlaceToCell(targetCell);
                return;
            }

            if (targetCell != null && targetCell.IsOccupied)
            {
                ItemData result = mergeHandler.TryMergeAndGetResult(itemView.Data, targetCell.Item.Data);

                if (result != null) {
                    MergeItems(targetCell, result);
                    return;
                }
            }

            ReturnToOriginalCell();
        }

        public void SetCell(GridCell cell)
        {
            originalCell = cell;
        }

        private Vector2 GetLocalPointerPosition(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint
            );

            return localPoint;
        }

        private void PlaceToCell(GridCell targetCell)
        {
            transform.SetParent(targetCell.transform);
            ((RectTransform)transform).anchoredPosition = Vector2.zero;

            targetCell.SetItem(itemView);
            SetCell(targetCell);
        }

        private void ReturnToOriginalCell()
        {
            transform.SetParent(originalCell.transform);
            ((RectTransform)transform).anchoredPosition = Vector2.zero;

            originalCell.SetItem(itemView);
        }

        private void MergeItems(GridCell targetCell, ItemData result)
        {
            animator.PlayMerge(
                GetComponent<ItemView>(),
                targetCell.Item,
                () => { targetCell.Item.SetData(result); },
                () => { Destroy(gameObject); }
            );
        }

        private void OnDestroy()
        {
            gridController.EndAnimation();
            sequence?.Kill();
        }
    }
}