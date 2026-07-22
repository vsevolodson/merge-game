using UnityEngine;

public class GridCell : MonoBehaviour
{
    private int row;
    private int column;

    public int Row => row;
    public int Column => column;

    public void Initialize(int row, int column)
    {
        this.row = row;
        this.column = column;
    }
}