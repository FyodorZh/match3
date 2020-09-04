﻿using System;
using System.Collections.Generic;
using Match3;

namespace Match3.Logic
{
    class Board : IBoard
    {
        private readonly Game _game;
        private readonly List<Grid> _grids = new List<Grid>();

        public event Action<ICellObjectObserver, ICellObserver> CellObjectOwnerChange;
        public event Action<ICellObjectObserver> CellObjectDestroy;

        public Board(Game game, IEnumerable<IGridData> gridData)
        {
            _game = game;
            foreach (var data in gridData)
            {
                _grids.Add(new Grid(game, this, new GridId(_grids.Count + 1), data));
            }
        }

        public void Tick(DeltaTime dTime)
        {
            foreach (var grid in _grids)
            {
                grid.Tick(dTime);
            }
        }

        public IEnumerable<IGrid> Grids => _grids;
        IEnumerable<IGridObserver> IBoardObserver.Grids => _grids;

        public IGrid GetGrid(GridId id)
        {
            int pos = id.Id - 1;
            if (pos >= 0 && pos < _grids.Count)
                return _grids[pos];
            return null;
        }

        IGridObserver IBoardObserver.GetGrid(GridId id)
        {
            return GetGrid(id);
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