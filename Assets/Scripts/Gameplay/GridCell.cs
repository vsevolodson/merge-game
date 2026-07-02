using UnityEngine;

namespace Gameplay
{
    public class GridCell : MonoBehaviour
    {
        [SerializeField] private ItemView currentItem;

        public bool IsOcupied => currentItem != null;

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