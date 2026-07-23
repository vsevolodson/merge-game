using UnityEngine;
using System;
using System.Collections.Generic;

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
    private ItemView[,] itemViews;
    private GridModel gridModel;

    private AnimatorHandler animator;

    public GridCell[,] Cells => cells;
    public GridModel Model => gridModel;

    public event Action SpawnButtonPressed;

    private void Awake()
    {
        animator = GetComponent<AnimatorHandler>();
    }

    private void GenerateGrid()
    {
        cells = new GridCell[rows, columns];
        itemViews = new ItemView[rows, columns];
        gridModel = new GridModel(rows, columns);

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                GridCell cell = Instantiate(cellPrefab, transform);
                cell.Initialize(row, column);

                cells[row, column] = cell;
            }
        }
    }

    public void StartNewGame()
    {
        GenerateGrid();
    }

    public void Load(SaveData saveData)
    {
        GenerateGrid();

        if (saveData != null)
        {
            Restore(saveData);
        }
    }

    public void SpawnRandomItem()
    {
        GridCell cell = GetRandomFreeCell();

        if (cell == null)
            return;

        SpawnItemOnCell(startItem, cell);

        SpawnButtonPressed?.Invoke();
    }

    private void SpawnItemOnCell(ItemData itemData, GridCell cell)
    {
        ItemView item = Instantiate(itemPrefab, cell.transform);

        item.Initialize(itemData, levelManager);

        DragHandler drag = item.GetComponent<DragHandler>();
        drag.Initialize(canvasRect, this, animator);
        drag.SetCell(cell);

        int row = cell.Row;
        int column = cell.Column;

        gridModel.SetItem(row, column, itemData);
        itemViews[row, column] = item;
    }

    private GridCell GetRandomFreeCell()
    {
        List<GridCell> freeCells = new();

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                if (gridModel.IsEmpty(row, column))
                {
                    freeCells.Add(cells[row, column]);
                }
            }
        }

        if (freeCells.Count == 0)
            return null;

        return freeCells[UnityEngine.Random.Range(0, freeCells.Count)];
    }

    public void ClearGrid()
    {
        if (cells == null)
            return;

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                if (gridModel.IsEmpty(row, column))
                    continue;

                Destroy(itemViews[row, column].gameObject);

                itemViews[row, column] = null;
                gridModel.ClearItem(row, column);
            }
        }
    }

    private void Restore(SaveData saveData)
    {
        if (saveData.cells.Count != rows * columns)
        {
            return;
        }

        for (int index = 0; index < rows * columns; index++)
        {
            CellSaveData cellData = saveData.cells[index];

            if (cellData.itemLevel == 0)
                continue;

            int row = index / columns;
            int column = index % columns;

            ItemData itemData = itemDatabase.GetItem(cellData.itemLevel);

            SpawnItemOnCell(itemData, cells[row, column]);
        }
    }

    public bool IsOccupied(int row, int column)
    {
        return gridModel.IsOccupied(row, column);
    }

    public ItemData GetItem(int row, int column)
    {
        return gridModel.GetItem(row, column);
    }

    public ItemView GetItemView(int row, int column)
    {
        return itemViews[row, column];
    }

    public void RemoveItem(int row, int column)
    {
        gridModel.ClearItem(row, column);
        itemViews[row, column] = null;
    }

    public void PlaceItem(int row, int column, ItemView item)
    {
        gridModel.SetItem(row, column, item.Data);
        itemViews[row, column] = item;
    }

    public void UpdateItem(int row, int column, ItemData itemData)
    {
        gridModel.SetItem(row, column, itemData);
    }
}