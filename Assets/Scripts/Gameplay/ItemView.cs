using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class ItemView : MonoBehaviour
{
    private ItemData itemData;
    private LevelManager levelManager;

    [SerializeField] private Image iconImage;

    public ItemData Data => itemData;

    public static event Action<ItemData> ItemCreated;
    public static event Action<ItemData> ItemChangedOrDestroyed;

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
        ItemChangedOrDestroyed?.Invoke(this.itemData);
        
        this.itemData = itemData;
        RefreshView();

        ItemCreated?.Invoke(itemData);
    }

    public void NotifyChangedOrDestroyed()
    {
        ItemChangedOrDestroyed?.Invoke(itemData);
    }
}