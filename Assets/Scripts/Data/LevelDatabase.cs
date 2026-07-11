using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "LevelDatabase",
    menuName = "Scriptable Objects/LevelDatabase")]
public class LevelDatabase : ScriptableObject
{
    [SerializeField] private List<LevelConfig> levels = new();

    public int Count => levels.Count;

    public LevelConfig GetLevel(int index)
    {
        if (index < 0 || index >= levels.Count)
        {
            return null;
        }

        return levels[index];
    }

    public bool HasLevel(int index)
    {
        return index >= 0 && index < levels.Count;;
    }
}