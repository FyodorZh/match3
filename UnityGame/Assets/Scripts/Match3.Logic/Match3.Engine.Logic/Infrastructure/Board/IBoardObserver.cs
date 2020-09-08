using System;
using System.Collections.Generic;

namespace Match3
{
    public interface IBoardObserver : IObserver
    {
        event Action<ICellObjectObserver, ICellObserver> CellObjectOwnerChange;
        event Action<ICellObjectObserver> CellObjectDestroy;

        int Width { get; }
        int Height { get; }

        IGameObserver Game { get; }

        IEnumerable<ICellObserver> AllCells { get; }

        ICellObserver GetCell(CellPosition position);
        IBorderObserver GetBorder(BorderPosition position);
    }
}