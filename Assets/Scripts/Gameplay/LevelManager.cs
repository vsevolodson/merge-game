using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelDatabase levelDatabase;
    [SerializeField] private GridController gridController;

    private int currentCount;
    private LevelConfig currentLevel;
    private int currentLevelIndex;
    private SaveService saveService;

    public event Action LevelCompleted;
    public event Action<int, int> ProgressChanged;
    public event Action<ItemData, int> LevelLoaded;

    public bool HasNextLevel => levelDatabase.HasLevel(currentLevelIndex + 1);
    public int CurrentLevelIndex => currentLevelIndex;

    private void Awake()
    {
        saveService = new SaveService();

        currentLevelIndex = 0;
        LoadLevel(currentLevelIndex);
    }

    private void OnEnable()
    {
        ItemView.ItemCreated += OnItemCreated;
        ItemView.ItemChangedOrDestroied += OnItemChangedOrDestroied;
        DragHandler.ItemDraged += Save;
        gridController.SpawnButtonPressed += Save;
        MergeHandler.ItemMerged += Save;
    }

    private void OnDisable()
    {
        ItemView.ItemCreated -= OnItemCreated;
        ItemView.ItemChangedOrDestroied -= OnItemChangedOrDestroied;
        DragHandler.ItemDraged -= Save;
        gridController.SpawnButtonPressed -= Save;
        MergeHandler.ItemMerged -= Save;
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

    public void OnItemChangedOrDestroied(ItemData item)
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

    private void Save()
    {
        SaveData saveData = CreateSaveData();

        saveService.Save(saveData);
    }

    private SaveData CreateSaveData()
    {
        SaveData saveData = new SaveData();

        saveData.currentLevel = currentLevelIndex;

        foreach (GridCell cell in gridController.Cells)
        {
            CellSaveData cellData = new CellSaveData();

            if (cell.IsOccupied)
            {
                cellData.itemLevel = cell.Item.Data.Level;
            }
            else
            {
                cellData.itemLevel = 0;
            }

            saveData.cells.Add(cellData);
        }

        return saveData;
    }
}