using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AnimatorHandler), typeof(CanvasGroup))]
public class GridController : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int rows = 5;
    [SerializeField] private int columns = 5;

    [Header("References")]
    [SerializeField] private GridCell cellPrefab;
    [SerializeField] private ItemView itemPrefab;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private ItemData startItem;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private ItemDatabase itemDatabase;

    private GridCell[,] cells;
    private AnimatorHandler animator;
    private SaveService saveService;

    public GridCell[,] Cells => cells;

    public event Action SpawnButtonPressed;

    private void Awake()
    {
        animator = GetComponent<AnimatorHandler>();
        saveService = new SaveService();
    }

    private void Start()
    {
        cells = new GridCell[rows, columns];
        GenerateGrid();

        SaveData saveData = saveService.Load();
        if (saveData != null)
        {
            Restore(saveData);
        }
    }

    private void GenerateGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                GridCell cell = Instantiate(cellPrefab, transform);
                cells[row, column] = cell;
            }
        }
    }

    public void SpawnRandomItem()
    {
        GridCell cell = GetRandomFreeCell();

        if (cell == null)
        {
            return;
        }

        SpawnItemOnCell(startItem, cell);

        SpawnButtonPressed.Invoke();
    }

    private void SpawnItemOnCell(ItemData itemData, GridCell cell)
    {
        ItemView item = Instantiate(itemPrefab, cell.transform);
        DragHandler dragHandler = item.GetComponent<DragHandler>();

        item.Initialize(itemData, levelManager);
        dragHandler.Initialize(canvasRect, this, animator);

        cell.SetItem(item);
        dragHandler.SetCell(cell);
    }

    private GridCell GetRandomFreeCell()
    {
        List<GridCell> freeCells = new();

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                GridCell cell = cells[row, column];

                if (!cell.IsOccupied)
                {
                    freeCells.Add(cell);
                }
            }
        }
        
        if (freeCells.Count == 0)
        {
            return null;
        }

        return freeCells[UnityEngine.Random.Range(0, freeCells.Count)];
    }

    public void ClearGrid()
    {
        if (cells == null)
        {
            return;
        }

        foreach (GridCell cell in cells)
        {
            if (!cell.IsOccupied)
            {
                continue;
            }

            Destroy(cell.Item.gameObject);
            cell.ClearItem();
        }
    }

    private void Restore(SaveData saveData)
    {
        for (int index = 0; index < rows * columns; index++)
        {
            CellSaveData cellData = saveData.cells[index];

            if (cellData.itemLevel == 0)
            {
                continue;
            }

            ItemData itemData = itemDatabase.GetItem(cellData.itemLevel);

            int row = index / rows;
            int column = index - (row * columns);
            GridCell cell = cells[row, column];

            SpawnItemOnCell(itemData, cell);
        }
    }
}