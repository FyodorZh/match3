using System.Collections.Generic;
using Match3;

public class TrivialGridData : ICellGridData
{
    
    
    public int Width { get; }
    public int Height { get; }

    public TrivialGridData(int with, int height)
    {
        Width = with;
        Height = height;
        
    }
    
    public IEnumerable<ICellObjectData> GetCellDataAt(int x, int y)
    {
        return null;
    }

    public IBorderObjectData GetBorderDataAt(int x, int y)
    {
        return null;
    }
}