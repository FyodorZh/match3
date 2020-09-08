using System.Collections.Generic;

namespace Match3
{
    public interface IBoard : IBoardObserver
    {
        new IGame Game { get; }

        new IEnumerable<ICell> AllCells { get; }

        new ICell GetCell(CellPosition position);
        new IBorder GetBorder(BorderPosition position);
    }
}