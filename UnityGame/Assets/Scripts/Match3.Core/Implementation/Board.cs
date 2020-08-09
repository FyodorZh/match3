using System.Collections.Generic;

namespace Match3.Core
{
    class Board : IBoard
    {
        private readonly Game _game;
        private readonly List<Grid> _grids = new List<Grid>();

        public Board(Game game, IEnumerable<ICellGridData> gridData)
        {
            _game = game;
            foreach (var data in gridData)
            {
                _grids.Add(new Grid(game, new GridId(_grids.Count + 1), data));
            }
        }
    }
}