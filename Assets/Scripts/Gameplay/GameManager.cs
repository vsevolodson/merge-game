using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridController gridController;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private MergeSoundController mergeSoundController;

    private SaveService saveService;

    private void Start()
    {
        saveService = new SaveService();

        SaveData saveData = saveService.Load();

        if (saveData == null)
        {
            levelManager.StartNewGame();
            gridController.StartNewGame();
            mergeSoundController.StartNewGame();

            return;
        }

        levelManager.Load(saveData);
        gridController.Load(saveData);
        mergeSoundController.Load(saveData);
    }

    private void OnEnable()
    {
        DragHandler.ItemDraged += Save;
        gridController.SpawnButtonPressed += Save;
        MergeHandler.ItemMerged += Save;
        mergeSoundController.SoundEnabledSwitched += Save;
    }

    private void OnDisable()
    {
        DragHandler.ItemDraged -= Save;
        gridController.SpawnButtonPressed -= Save;
        MergeHandler.ItemMerged -= Save;
        mergeSoundController.SoundEnabledSwitched -= Save;
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

        saveData.soundEnabled = mergeSoundController.SoundEnabled;Debug.Log(saveData.soundEnabled);

        foreach (GridCell cell in gridController.Cells)
        {
            CellSaveData cellData = new CellSaveData();

            ItemData item = gridController.GetItem(cell.Row, cell.Column);

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