using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(ItemView), typeof(MergeHandler))]
public class DragHandler :
    MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    private MergeHandler mergeHandler;
    private ItemView itemView;

    private GridCell originalCell;
    private RectTransform canvasRect;
    private GridController gridController;

    private Vector2 localPoint;
    private Vector2 dragOffset;

    public static event Action ItemDraged;

    public void Initialize(
        RectTransform canvasRect,
        GridController gridController,
        AnimatorHandler animator)
    {
        this.canvasRect = canvasRect;
        this.gridController = gridController;

        mergeHandler.Initialize(animator, gridController);
    }

    private void Awake()
    {
        itemView = GetComponent<ItemView>();
        mergeHandler = GetComponent<MergeHandler>();
    }

    public void SetCell(GridCell cell)
    {
        originalCell = cell;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        gridController.RemoveItem(originalCell.Row, originalCell.Column);

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
        if (IsTrashCell(eventData))
        {
            RemoveItem();
            return;
        }

        GridCell targetCell = GetTargetCell(eventData);

        if (targetCell == null)
        {
            ReturnToOriginalCell();
            ItemDraged?.Invoke();
            return;
        }

        if (!gridController.IsOccupied(targetCell.Row, targetCell.Column))
        {
            PlaceToCell(targetCell);
            ItemDraged?.Invoke();
            return;
        }

        ItemData result = mergeHandler.TryMergeAndGetResult(
            itemView.Data,
            gridController.GetItem(targetCell.Row, targetCell.Column));

        if (result != null)
        {
            mergeHandler.MergeItems(targetCell, result);
            ItemDraged?.Invoke();
            return;
        }

        ReturnToOriginalCell();
        ItemDraged?.Invoke();
    }

    private GridCell GetTargetCell(PointerEventData eventData)
    {
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            GridCell cell = result.gameObject.GetComponent<GridCell>();

            if (cell != null)
                return cell;
        }

        return null;
    }

    private Vector2 GetLocalPointerPosition(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 point);

        return point;
    }

    private void PlaceToCell(GridCell targetCell)
    {
        transform.SetParent(targetCell.transform);
        ((RectTransform)transform).anchoredPosition = Vector2.zero;

        gridController.PlaceItem(targetCell.Row, targetCell.Column, itemView);
        SetCell(targetCell);
    }

    private void ReturnToOriginalCell()
    {
        transform.SetParent(originalCell.transform);
        ((RectTransform)transform).anchoredPosition = Vector2.zero;

        gridController.PlaceItem(originalCell.Row, originalCell.Column, itemView);
    }

    private void RemoveItem()
    {
        Destroy(gameObject);
        itemView.NotifyChangedOrDestroyed();
        ItemDraged?.Invoke();
    }

    private bool IsTrashCell(PointerEventData eventData)
    {
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<TrashCell>() != null)
            {
                return true;
            }
        }

        return false;
    }
}