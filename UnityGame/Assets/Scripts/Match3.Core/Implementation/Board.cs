using System;
using System.Collections.Generic;
using Match3.Math;

namespace Match3.Core
{
    class Board : IBoard
    {
        private readonly Game _game;
        private readonly List<Grid> _grids = new List<Grid>();

        public event Action<ICellObject, ICell> CellObjectOwnerChange;
        public event Action<ICellObject> CellObjectDestroy;
        
        public Board(Game game, IEnumerable<IGridData> gridData)
        {
            _game = game;
            foreach (var data in gridData)
            {
                _grids.Add(new Grid(game, this, new GridId(_grids.Count + 1), data));
            }
        }

        public void Tick(Fixed dTimeSeconds)
        {
            foreach (var grid in _grids)
            {
                grid.Tick(dTimeSeconds);
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

        public void OnCellObjectOwnerChange(ICellObject cellObject, ICell oldOwner)
        {
            CellObjectOwnerChange?.Invoke(cellObject, oldOwner);
        }
        
        public void OnCellObjectDestroy(ICellObject cellObject)
        {
            CellObjectDestroy?.Invoke(cellObject);
        }
    }
}