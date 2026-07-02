using UnityEngine;

namespace Gameplay
{    
    public class GridController : MonoBehaviour
    {
        [Header("Grid Settings")]
        [SerializeField] private int rows = 5;
        [SerializeField] private int columns = 5;

        [Header("References")]
        [SerializeField] private GridCell cellPrefab;

        private void Start()
        {
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    Instantiate(cellPrefab, transform);
                }
            }
        }
    }
}