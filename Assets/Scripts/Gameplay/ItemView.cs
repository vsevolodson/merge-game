using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    private ItemData itemData;
    private LevelManager levelManager;

    [SerializeField] private Image iconImage;

    public ItemData Data => itemData;

    public void Initialize(ItemData itemData, LevelManager levelManager)
    {
        this.levelManager = levelManager;

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

        levelManager.OnItemCreated(itemData);
    }
}