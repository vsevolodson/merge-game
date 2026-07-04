using UnityEngine;
[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Visual")]
    [SerializeField] private Sprite icon;

    [Header("Gameplay")]
    [SerializeField] private int level;

    [SerializeField] private ItemData nextLevel;
    [SerializeField] private ItemType type;

    public ItemType Type => type;
    public Sprite Icon => icon;
    public int Level => level;
    public ItemData NextLevel => nextLevel;
}