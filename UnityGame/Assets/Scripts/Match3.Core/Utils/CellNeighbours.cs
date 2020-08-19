using System.Collections.Generic;

namespace Match3.Core
{
    public interface ICellNeighbours
    {
        IEnumerable<ICell> Enumerate(ICell cell, bool active);
    }

    public class CellNeighbours : ICellNeighbours
    {
        private readonly Pattern2D _pattern2D;

        public CellNeighbours(Pattern2D pattern)
        {
            _pattern2D = pattern;
        }

        public IEnumerable<ICell> Enumerate(ICell cell, bool active)
        {
            var pos = cell.Position;
            int x0 = pos.X;
            int y0 = pos.Y;

            var grid = cell.Owner;
            for (int i = 0; i < _pattern2D.Length; ++i)
            {
                int x = x0 + _pattern2D.OffsetsX[i];
                int y = y0 + _pattern2D.OffsetsY[i];

                var neighbour = grid.GetCell(new CellPosition(x, y));
                if (neighbour != null)
                {
                    if (!active || neighbour.IsActive)
                    {
                        yield return neighbour;
                    }
                }
            }
        }
    }
}