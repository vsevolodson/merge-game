using UnityEngine;
using System;

public class MergeHandler : MonoBehaviour
{
    private AnimatorHandler animator;

    public static event Action ItemMerged;

    public void Initialize(AnimatorHandler animator)
    {
        this.animator = animator;
    }

    public ItemData TryMergeAndGetResult(ItemData first, ItemData second)
    {
        if (first == null || second == null)
        {
            return null;
        }

        if (first != second)
        {
            return null;
        }

        return first.NextLevel;
    }

    public void MergeItems(GridCell targetCell, ItemData result)
    {
        ItemView item = GetComponent<ItemView>();
        animator.PlayMerge(
            item,
            targetCell.Item,
            () => { targetCell.Item.SetData(result); },
            () => 
                {
                    Destroy(gameObject);
                    item.NotifyChangedOrDestroyed();
                    ItemMerged.Invoke();
                }
        );
    }
}