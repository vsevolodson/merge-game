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
    public int CurrentLevelIndex => currentLevelIndex;
    public int CurrentCount => currentCount;

    private void OnEnable()
    {
        ItemView.ItemCreated += OnItemCreated;
        ItemView.ItemChangedOrDestroyed += OnItemChangedOrDestroyed;
    }

    private void OnDisable()
    {
        ItemView.ItemCreated -= OnItemCreated;
        ItemView.ItemChangedOrDestroyed -= OnItemChangedOrDestroyed;
    }

    public void OnItemCreated(ItemData item)
    {
        if (item != currentLevel.TargetItem)
        {
            return;
        }

        currentCount++;

        ChangeProgress();
    }

    public void OnItemChangedOrDestroyed(ItemData item)
    {        
        if (item != currentLevel.TargetItem)
        {
            return;
        }

        currentCount--;

        ChangeProgress();
    }

    private void ChangeProgress()
    {
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

    private bool LoadLevel(int index)
    {
        currentLevelIndex = index;

        gridController.ClearGrid();

        currentLevel = levelDatabase.GetLevel(index);
        if (currentLevel == null)
        {
            return false;
        }

        currentCount = 0;
        LevelLoaded?.Invoke(currentLevel.TargetItem, currentLevel.TargetCount);
        ChangeProgress();

        return true;
    }

    public void LoadNextLevel()
    {
        currentLevelIndex++;
        LoadLevel(currentLevelIndex);
    }

    public void StartNewGame()
    {
        currentLevelIndex = 0;
        LoadLevel(currentLevelIndex);
    }

    public bool Load(SaveData saveData)
    {
        if (saveData == null)
        {
            return false;
        }

        return LoadLevel(saveData.currentLevel);
    }
}