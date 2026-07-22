public class GridModel
{
    private readonly ItemData[,] items;

    public int Rows { get; }
    public int Columns { get; }

    public GridModel(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;

        items = new ItemData[rows, columns];
    }

    public ItemData GetItem(int row, int column)
    {
        return items[row, column];
    }

    public void SetItem(int row, int column, ItemData item)
    {
        items[row, column] = item;
    }

    public void ClearItem(int row, int column)
    {
        items[row, column] = null;
    }

    public bool IsOccupied(int row, int column)
    {
        return items[row, column] != null;
    }

    public bool IsEmpty(int row, int column)
    {
        return items[row, column] == null;
    }
}