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

    private void LoadLevel(int index)
    {
        currentLevelIndex = index;

        gridController.ClearGrid();

        currentLevel = levelDatabase.GetLevel(index);
        if (currentLevel == null)
        {
            Debug.Log("Все уровни пройдены или данные неверные");
            return;
        }

        currentCount = 0;
        LevelLoaded?.Invoke(currentLevel.TargetItem, currentLevel.TargetCount);
        ChangeProgress();
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

    public void Load(SaveData saveData)
    {
        LoadLevel(saveData.currentLevel);
    }
}