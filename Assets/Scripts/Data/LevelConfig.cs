using UnityEngine;

[CreateAssetMenu(
    fileName = "LevelConfig",
    menuName = "Scriptable Objects/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    [SerializeField] private ItemData targetItem;
    [SerializeField] private int targetCount;

    public ItemData TargetItem => targetItem;
    public int TargetCount => targetCount;
}