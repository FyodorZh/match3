using System;
using System.Collections.Generic;

namespace Match3
{
    public interface IBoardObserver : IObserver
    {
        event Action<ICellObjectObserver, ICellObserver> CellObjectOwnerChange;
        event Action<ICellObjectObserver> CellObjectDestroy;

        IEnumerable<IGridObserver> Grids { get; }
        IGridObserver GetGrid(GridId id);
    }
}