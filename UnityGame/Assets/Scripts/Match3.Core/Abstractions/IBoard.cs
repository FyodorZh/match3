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
    
    public static class IBoard_Ext
    {
        public static ICell GetCell(this IBoard board, CellId cellId)
        {
            IGrid grid = board.GetGrid(cellId.GridId);
            return grid?.GetCell(cellId.Position);
        }
    }
}