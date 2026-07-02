using UnityEngine;
using System.Collections.Generic;

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

        private GridCell[,] cells;

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
            cell.SetItem(item);
        }

        private GridCell GetRandomFreeCell()
        {
            List<GridCell> freeCells = new();

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    GridCell cell = cells[row, column];

                    if (!cell.IsOcupied)
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
    }
}