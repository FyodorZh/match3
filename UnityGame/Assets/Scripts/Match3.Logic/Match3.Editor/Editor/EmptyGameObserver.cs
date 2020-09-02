#pragma warning disable CS0067 // Event is not invoked

using System;
using System.Collections.Generic;

namespace Match3.Editor
{
    class EmptyGameObserver : IGameObserver, IBoardObserver
    {
        public static readonly EmptyGameObserver Instance = new EmptyGameObserver();

        public IBoardObserver Board => this;

        public event Action<ICellObjectObserver, ICellObserver> CellObjectOwnerChange;
        public event Action<ICellObjectObserver> CellObjectDestroy;

        public IEnumerable<IGridObserver> Grids { get; } = new IGridObserver[0];
        public IGridObserver GetGrid(GridId id)
        {
            return null;
        }
    }
}