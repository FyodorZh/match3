using System.Collections.Generic;

namespace Match3.Core
{
    class Board : IBoard
    {
        private readonly Game _game;
        private readonly List<Grid> _grids = new List<Grid>();

        public Board(Game game, IEnumerable<IGridData> gridData)
        {
            _game = game;
            foreach (var data in gridData)
            {
                _grids.Add(new Grid(game, new GridId(_grids.Count + 1), data));
            }
        }

        public IEnumerable<IGrid> Grids => _grids;
        
        public IGrid GetGrid(GridId id)
        {
            int pos = id.Id - 1;
            if (pos >= 0 && pos < _grids.Count)
                return _grids[pos];
            return null;
        }
    }
}