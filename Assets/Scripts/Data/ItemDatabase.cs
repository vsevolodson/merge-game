using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] private List<ItemData> items;

    public ItemData GetItem(int level)
    {
        foreach (ItemData item in items)
        {
            if (item.Level == level)
            {
                return item;
            }
        }

        return null;
    }
}