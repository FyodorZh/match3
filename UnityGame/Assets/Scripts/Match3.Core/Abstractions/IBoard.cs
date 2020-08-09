using System.Collections.Generic;

namespace Match3
{
    public interface IBoard
    {
        IEnumerable<IGrid> Grids { get; }
        IGrid GetGrid(GridId id);
    }
}