using System.Collections.Generic;

namespace Match3
{
    public interface ICellGridData
    {
        int Width { get; }
        int Height { get; }

        IEnumerable<ICellObjectData> GetCellDataAt(int x, int y);
        
        IBorderObjectData GetBorderDataAt(int x, int y);
    }
}