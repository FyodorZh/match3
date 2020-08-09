using System.Collections.Generic;

namespace Match3.Core
{
    class Board : IBoard
    {
        private readonly List<Grid> _grids = new List<Grid>();

        public Board(IEnumerable<ICellGridData> gridData)
        {
            foreach (var data in gridData)
            {
                _grids.Add(new Grid(new GridId(_grids.Count + 1), data));
            }
        }
    }
}