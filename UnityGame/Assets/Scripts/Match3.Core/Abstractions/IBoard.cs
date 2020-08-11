using System;
using System.Collections.Generic;

namespace Match3
{
    public interface IBoard
    {
        event Action<ICellObject, ICell> CellObjectOwnerChange;
        event Action<ICellObject> CellObjectDestroy;
        
        IEnumerable<IGrid> Grids { get; }
        IGrid GetGrid(GridId id);
    }
}