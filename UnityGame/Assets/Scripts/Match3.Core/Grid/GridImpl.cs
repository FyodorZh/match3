using System.Diagnostics;

namespace Match3.Core
{
    class Grid : IGrid
    {
        private readonly Game _game;
        private readonly Cell[,] _cells;
        private readonly Border[,] _borders;
        
        public GridId Id { get; }
        public int Width { get; }
        public int Height { get; }
        
        public Grid(Game game, GridId id, ICellGridData data)
        {
            int width = data.Width;
            int height = data.Height;

            _game = game;
            _cells = new Cell[width, height];
            _borders = new Border[width + 1, height + 1];
            Width = width;
            Height = height;
            
            var rules = game.Rules;
            var objectFactory = rules.ObjectFactory;
            
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    _cells[x, y] = new Cell(this, new CellPosition(x, y));
                    var list = data.GetCellDataAt(x, y);
                    if (list != null)
                    {
                        var cell = _cells[x, y];
                        cell.IsActive = true;
                        foreach (var objectData in list)
                        {
                            var obj = objectFactory.Construct<ICellObject>(objectData, _game);
                            Debug.Assert(obj != null);
                            if (!cell.TryAddContent(obj))
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
                        _borders[x, y].SetContent(objectFactory.Construct<IBorderObject>(borderData, _game));
                    }
                }
            }
        }
        
        public ICell GetCell(CellPosition position)
        {
            return CellAt(position.X, position.Y);
        }

        public IBorder GetBorder(BorderPosition position)
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