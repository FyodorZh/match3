using System.Collections.Generic;

namespace Match3.Editor
{
    public interface ILevelData
    {
        string Name { get; }
        IEnumerable<IGridData> Grids { get; }
    }
}