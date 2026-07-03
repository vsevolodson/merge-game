using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gameplay
{
    public class ItemView : MonoBehaviour
    {
        private ItemData itemData;

        [SerializeField] private Image iconImage;

        public ItemData Data => itemData;

        public void Initialize(ItemData itemData)
        {
            SetData(itemData);
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