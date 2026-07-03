using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Gameplay
{
    public class ItemView :
        MonoBehaviour,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler
    {
        private GridCell originalCell;
        private RectTransform canvasRect;
        private Vector2 localPoint;
        private Vector2 dragOffset;
        private ItemData itemData;

        [SerializeField] private Image iconImage;

        public ItemData Data => itemData;

        public void Initialize(RectTransform canvasRect, ItemData itemData)
        {
            this.canvasRect = canvasRect;
            SetData(itemData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            originalCell.ClearItem();
            transform.SetParent(canvasRect);
            transform.SetAsLastSibling();

            localPoint = GetLocalPointerPosition(eventData);
            dragOffset = ((RectTransform)transform).anchoredPosition - localPoint;
        }

        public void OnDrag(PointerEventData eventData)
        {
            localPoint = GetLocalPointerPosition(eventData);
            ((RectTransform)transform).anchoredPosition = localPoint + dragOffset;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
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
                ReturnToOriginalCell();
                return;
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

            targetCell.SetItem(this);
            SetCell(targetCell);
        }

        private void ReturnToOriginalCell()
        {
            transform.SetParent(originalCell.transform);
            ((RectTransform)transform).anchoredPosition = Vector2.zero;

            originalCell.SetItem(this);
        }

        private void RefreshView()
        {
            iconImage.sprite = itemData.Icon;
        }

        public void SetData(ItemData itemData)
        {
            this.itemData = itemData;
            RefreshView();
        }
    }
}