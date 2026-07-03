using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Gameplay
{    
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

        private GridCell[,] cells;
        private bool isAnimating;

        public bool IsAnimating => isAnimating;

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

            item.Initialize(startItem);
            dragHandler.Initialize(canvasRect, this);

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
        }

        public void EndAnimation()
        {
            isAnimating = false;
        }
    }
}