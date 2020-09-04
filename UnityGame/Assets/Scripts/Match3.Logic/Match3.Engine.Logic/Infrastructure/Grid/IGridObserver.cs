using System.Collections.Generic;

namespace Match3
{
    public interface IGridObserver : IObserver
    {
        GridId Id { get; }
        int Width { get; }
        int Height { get; }

        IGameObserver Game { get; }

        IEnumerable<ICellObserver> AllCells { get; }

        ICellObserver GetCell(CellPosition position);
        IBorderObserver GetBorder(BorderPosition position);
    }
}