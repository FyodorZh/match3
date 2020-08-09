namespace Match3.Core
{
    class CellGrid : ICellGrid
    {
        private readonly Cell[,] _cells;
        private readonly Border[,] _borders;
        
        public CellGridId Id { get; }
        public int Width { get; }
        public int Height { get; }
        
        public CellGrid(CellGridId id, int width, int height)
        {
            _cells = new Cell[width, height];
            _borders = new Border[width + 1, height + 1];
            Width = width;
            Height = height;
            
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    _cells[x, y] = new Cell(this, new CellPosition(x, y));
                }
            }
            
            for (int x = 0; x <= width; ++x)
            {
                for (int y = 0; y <= height; ++y)
                {
                    _borders[x, y] = new Border(this, new BorderPosition(x, y));
                }
            }
        }

        public CellGrid(CellGridId id, ICellGridData data)
            : this(id, data.Width, data.Height)
        {
            // todo
        }
        
        public ICell GetCell(CellPosition position)
        {
            return At(position.X, position.Y);
        }

        public IBorder GetBorder(BorderPosition position)
        {
            throw new System.NotImplementedException();
        }

        private Cell At(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
                return _cells[x, y];
            return null; // TODO
        }
    }
}