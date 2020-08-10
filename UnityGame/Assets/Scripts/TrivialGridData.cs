using System.Collections.Generic;
using Match3;

public class TrivialGridData : IGridData
{
    private readonly List<ICellObjectData>[,] _cells;
    
    public int Width { get; }
    public int Height { get; }

    public TrivialGridData(int with, int height)
    {
        Width = with;
        Height = height;
        _cells = new List<ICellObjectData>[Width, Height];
    }

    public void ActivateCell(int x, int y)
    {
        if (_cells[x, y] == null)
        {
            _cells[x, y] = new List<ICellObjectData>();
        }
    }

    public void AddCellContent(int x, int y, ICellObjectData data)
    {
        ActivateCell(x, y);
        _cells[x, y].Add(data);
    }
    
    public IEnumerable<ICellObjectData> GetCellDataAt(int x, int y)
    {
        return _cells[x, y];
    }

    public IBorderObjectData GetBorderDataAt(int x, int y)
    {
        return null;
    }
}