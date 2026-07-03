using UnityEngine;

namespace Gameplay
{
    public class GridCell : MonoBehaviour
    {
        [SerializeField] private ItemView currentItem;

        public bool IsOccupied => currentItem != null;
        public ItemView Item => currentItem;

        public void SetItem(ItemView item)
        {
            currentItem = item;
        }

        public void ClearItem()
        {
            currentItem = null;
        }
    }
}