using System.Collections.Generic;

namespace Match3
{
    public interface ICellObserver : IObserver
    {
        CellPosition Position { get; }

        IGameObserver Game { get; }
        IBoardObserver Owner { get; }

        bool IsActive { get; }

        bool IsLocked { get; }

        IEnumerable<ICellObjectObserver> Objects { get; }
    }
}