using System;

namespace Match3
{
    public interface IGame : IGameObserver, IGameContext
    {
        new IBoard Board { get; }

        void InternalInvoke(Action action);
    }
}