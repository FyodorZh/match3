using System.Collections.Generic;

namespace Match3
{
    public interface ICellNeighbours
    {
        IEnumerable<ICell> Enumerate(ICell cell, bool active);
    }

    public class CellNeighbours : ICellNeighbours
    {
        private readonly Offsets2D _pattern2D;

        public CellNeighbours(Offsets2D pattern)
        {
            _pattern2D = pattern;
        }

        public IEnumerable<ICell> Enumerate(ICell cell, bool active)
        {
            var pos0 = cell.Position;

            var grid = cell.Owner;
            for (int i = 0; i < _pattern2D.Length; ++i)
            {
                var offset = _pattern2D.OffsetAt(i);
                var pos = pos0 + _pattern2D.OffsetAt(i);

                var neighbour = grid.GetCell(pos);
                if (neighbour != null)
                {
                    if (!active || neighbour.IsActive)
                    {
                        yield return neighbour;
                    }
                }
            }
        }

        public static readonly CellNeighbours PatternCross = new CellNeighbours(new Offsets2D(new []
        {
            new[] {-1, 0},
            new[] {0, -1},
            new[] {1, 0},
            new[] {0, 1}
        }));

        public static readonly CellNeighbours PatternBox = new CellNeighbours(new Offsets2D(new []
        {
            new[] {-1, -1},
            new[] {-1, 0},
            new[] {-1, 1},
            new[] {0, -1},
            new[] {0, 1},
            new[] {1, -1},
            new[] {1, 0},
            new[] {1, 1},
        }));
    }
}