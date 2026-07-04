using UnityEngine;

public class MergeHandler : MonoBehaviour
{
    private AnimatorHandler animator;

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
        animator.PlayMerge(
            GetComponent<ItemView>(),
            targetCell.Item,
            () => { targetCell.Item.SetData(result); },
            () => { Destroy(gameObject); }
        );
    }
}