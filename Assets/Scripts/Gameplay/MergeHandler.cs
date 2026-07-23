using UnityEngine;
using System;

public class MergeHandler : MonoBehaviour
{
    private AnimatorHandler animator;
    private GridController gridController;

    public static event Action ItemMerged;
    public static event Action<Vector3> MergePerformed;

    public void Initialize(
        AnimatorHandler animator,
        GridController gridController)
    {
        this.animator = animator;
        this.gridController = gridController;
    }

    public ItemData TryMergeAndGetResult(ItemData first, ItemData second)
    {
        if (first == null || second == null)
            return null;

        if (first != second)
            return null;

        return first.NextLevel;
    }

    public void MergeItems(GridCell targetCell, ItemData result)
    {
        ItemView movingItem = GetComponent<ItemView>();
        ItemView targetItem = gridController.GetItemView(targetCell.Row, targetCell.Column);

        animator.PlayMerge(
            movingItem,
            targetItem,
            () =>
            {
                targetItem.SetData(result);
                gridController.UpdateItem(targetCell.Row, targetCell.Column, result);
            },
            () =>
            {
                Destroy(gameObject);
                movingItem.NotifyChangedOrDestroyed();
                MergePerformed?.Invoke(targetItem.transform.position);      
                ItemMerged?.Invoke();
            });
    }
}