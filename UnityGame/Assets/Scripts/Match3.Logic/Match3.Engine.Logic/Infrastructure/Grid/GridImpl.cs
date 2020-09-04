﻿using System.Collections.Generic;

namespace Match3.Logic
{
    class Grid : IGrid
    {
        private readonly Game _game;
        private readonly Cell[,] _cells;
        private readonly Border[,] _borders;

        private readonly Cell[] _cellList;

        public Board Board { get; }

        public GridId Id { get; }
        public int Width { get; }
        public int Height { get; }
        public IGame Game => _game;
        IGameObserver IGridObserver.Game => _game;

        public Grid(Game game, Board board, GridId id, IGridData data)
        {
            int width = data.Width;
            int height = data.Height;

            _game = game;
            Board = board;
            Id = id;
            _cells = new Cell[width, height];
            _borders = new Border[width + 1, height + 1];
            Width = width;
            Height = height;

            var rules = game.Rules;
            var objectFactory = rules.ObjectFactory;

            int pos = 0;
            _cellList = new Cell[Width * Height];
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    var cell = new Cell(this, new CellPosition(x, y));

                    _cells[x, y] = cell;
                    _cellList[pos++] = cell;

                    var list = data.GetCellDataAt(x, y);
                    if (list != null)
                    {
                        cell.IsActive = true;
                        foreach (var objectData in list)
                        {
                            var obj = objectFactory.Construct<ICellObject>(objectData);
                            Debug.Assert(obj != null);
                            if (cell.CanAttach(obj))
                            {
                                cell.Attach(obj);
                            }
                            else
                            {
                                Debug.Assert(false);
                            }
                        }
                    }
                }
            }

            for (int x = 0; x <= width; ++x)
            {
                for (int y = 0; y <= height; ++y)
                {
                    _borders[x, y] = new Border(this, new BorderPosition(x, y));
                    var borderData = data.GetBorderDataAt(x, y);
                    if (borderData != null)
                    {
                        _borders[x, y].SetContent(objectFactory.Construct<IBorderObject>(borderData));
                    }
                }
            }
        }

        public void Tick(DeltaTime dTime)
        {
            foreach (var cell in _cellList)
            {
                cell.Tick(dTime);
            }
        }

        public IEnumerable<ICell> AllCells => _cellList;
        IEnumerable<ICellObserver> IGridObserver.AllCells => _cellList;

        public ICell GetCell(CellPosition position)
        {
            return CellAt(position.X, position.Y);
        }

        ICellObserver IGridObserver.GetCell(CellPosition position)
        {
            return GetCell(position);
        }

        public IBorder GetBorder(BorderPosition position)
        {
            return BorderAt(position.X, position.Y);
        }

        IBorderObserver IGridObserver.GetBorder(BorderPosition position)
        {
            return BorderAt(position.X, position.Y);
        }

        private Cell CellAt(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
                return _cells[x, y];
            return null; // TODO
        }

        private Border BorderAt(int x, int y)
        {
            if (x >= 0 && x <= Width && y >= 0 && y <= Height)
                return _borders[x, y];
            return null; // TODO
        }
    }
}