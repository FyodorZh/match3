using System.Collections.Generic;

namespace Match3.Core
{
    class Board : IBoard
    {
        private readonly List<CellGrid> _grids = new List<CellGrid>();

        public Board(IEnumerable<ICellGridData> gridData)
        {
            foreach (var data in gridData)
            {
                _grids.Add(new CellGrid(new CellGridId(_grids.Count + 1), data));
            }
        }
    }
}