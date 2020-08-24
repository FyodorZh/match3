using System;
using System.Collections.Generic;

namespace Match3
{
    public interface IBoard : IBoardObserver
    {
        // event Action<ICellObject, ICell> CellObjectOwnerChange;
        // event Action<ICellObject> CellObjectDestroy;

        new IEnumerable<IGrid> Grids { get; }
        new IGrid GetGrid(GridId id);
    }
}