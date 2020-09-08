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

        int IBoardObserver.Width => 0;

        int IBoardObserver.Height => 0;

        IGameObserver IBoardObserver.Game => this;

        IEnumerable<ICellObserver> IBoardObserver.AllCells => new ICellObserver[0];

        ICellObserver IBoardObserver.GetCell(CellPosition position)
        {
            return null;
        }

        IBorderObserver IBoardObserver.GetBorder(BorderPosition position)
        {
            return null;
        }
    }
}