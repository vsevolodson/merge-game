using UnityEngine;

namespace Gameplay
{
    public class GridCell : MonoBehaviour
    {
        [Header("Grid Settings")]
        [SerializeField] private int rows = 5;
        [SerializeField] private int columns = 5;

        [Header("References")]
        [SerializeField] private GridCell cellPrefab;

    }
}