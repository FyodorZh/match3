using System.Collections.Generic;

namespace Match3
{
    public interface IGridData
    {
        int PosX { get; }
        int PosY { get; }

        int Width { get; }
        int Height { get; }

        IEnumerable<ICellObjectData> GetCellDataAt(int x, int y);

        IBorderObjectData GetBorderDataAt(int x, int y);
    }
}