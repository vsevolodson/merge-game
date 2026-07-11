using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelDatabase levelDatabase;
    [SerializeField] private GridController gridController;

    private int currentCount;
    private LevelConfig currentLevel;
    private int currentLevelIndex;

    public event Action LevelCompleted;
    public event Action<int, int> ProgressChanged;
    public event Action<ItemData, int> LevelLoaded;

    public bool HasNextLevel => levelDatabase.HasLevel(currentLevelIndex + 1);

    private void Awake()
    {
        currentLevelIndex = 0;
        LoadLevel(currentLevelIndex);
    }

    private void OnEnable()
    {
        ItemView.ItemCreated += OnItemCreated;
    }

    private void OnDisable()
    {
        ItemView.ItemCreated -= OnItemCreated;
    }

    public void OnItemCreated(ItemData item)
    {
        if (item != currentLevel.TargetItem)
        {
            return;
        }

        currentCount++;

        ProgressChanged?.Invoke(currentCount, currentLevel.TargetCount);

        if (currentCount >= currentLevel.TargetCount)
        {
            CompleteLevel();
        }
    }

    private void CompleteLevel()
    {
        LevelCompleted?.Invoke();
    }

    private void LoadLevel(int index)
    {
        gridController.ClearGrid();

        currentLevel = levelDatabase.GetLevel(index);
        if (currentLevel == null)
        {
            Debug.Log("Все уровни пройдены");
            return;
        }

        currentCount = 0;
        LevelLoaded?.Invoke(currentLevel.TargetItem, currentLevel.TargetCount);
        ProgressChanged?.Invoke(currentCount, currentLevel.TargetCount);
    }

    public void LoadNextLevel()
    {
        currentLevelIndex++;
        LoadLevel(currentLevelIndex);
    }
}