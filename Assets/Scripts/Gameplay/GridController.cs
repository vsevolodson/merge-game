using UnityEngine;
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

    private GridCell[,] cells;
    private bool isAnimating;
    private AnimatorHandler animator;
    private CanvasGroup group;

    public bool IsAnimating => isAnimating;

    private void Awake()
    {
        animator = GetComponent<AnimatorHandler>();
        group = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        cells = new GridCell[rows, columns];
        GenerateGrid();
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

        ItemView item = Instantiate(itemPrefab, cell.transform);
        DragHandler dragHandler = item.GetComponent<DragHandler>();

        item.Initialize(startItem, levelManager);
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

        return freeCells[Random.Range(0, freeCells.Count)];
    }

    public void BeginAnimation()
    {
        isAnimating = true;
        
        group.interactable = false;
        group.blocksRaycasts = false;
    }

    public void EndAnimation()
    {
        isAnimating = false;

        group.interactable = true;
        group.blocksRaycasts = true;
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
}