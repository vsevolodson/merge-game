using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridController gridController;
    [SerializeField] private LevelManager levelManager;

    private SaveService saveService;

    private void Start()
    {
        saveService = new SaveService();

        SaveData saveData = saveService.Load();

        if (saveData == null)
        {
            levelManager.StartNewGame();
            gridController.StartNewGame();

            return;
        }

        levelManager.Load(saveData);
        gridController.Load(saveData);
    }

    private void OnEnable()
    {
        DragHandler.ItemDraged += Save;
        gridController.SpawnButtonPressed += Save;
        MergeHandler.ItemMerged += Save;
    }

    private void OnDisable()
    {
        DragHandler.ItemDraged -= Save;
        gridController.SpawnButtonPressed -= Save;
        MergeHandler.ItemMerged -= Save;
    }

    private void Save()
    {
        SaveData saveData = CreateSaveData();

        saveService.Save(saveData);
    }

    private SaveData CreateSaveData()
    {
        SaveData saveData = new SaveData();

        saveData.currentLevel = levelManager.CurrentLevelIndex;

        foreach (GridCell cell in gridController.Cells)
        {
            CellSaveData cellData = new CellSaveData();

            ItemData item = gridController.GetItem(cell);

            if (item != null)
            {
                cellData.itemLevel = item.Level;
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