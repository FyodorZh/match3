using System.Collections.Generic;

namespace Match3
{
    public interface IGrid
    {
        GridId Id { get; }
        int Width { get; }
        int Height { get; }
        
        IEnumerable<ICell> AllCells { get; }

        ICell GetCell(CellPosition position);
        IBorder GetBorder(BorderPosition position);
    }
}