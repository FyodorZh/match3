using System.Collections.Generic;

namespace Match3
{
    public interface ICellObserver
    {
        CellId Id { get; }
        CellPosition Position { get; }

        IGameObserver Game { get; }
        IGridObserver Owner { get; }

        bool IsActive { get; }

        bool IsLocked { get; }

        IEnumerable<ICellObjectObserver> Objects { get; }
    }
}