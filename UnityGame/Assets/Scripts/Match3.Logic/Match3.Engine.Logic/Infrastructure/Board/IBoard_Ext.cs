using System;
using System.Collections.Generic;

namespace Match3
{
    public static class IBoard_Ext
    {
        public static ICell GetCell(this IBoard board, CellId cellId)
        {
            IGrid grid = board.GetGrid(cellId.GridId);
            return grid?.GetCell(cellId.Position);
        }
    }
}