using System;

namespace Match3
{
    public interface IGame : IGameContext
    {
        IGameRules Rules { get; }
        IBoard Board { get; }
        void Tick(int dTimeMs);

        void Action(string actionFeatureName, params CellId[] cells);

        void InternalInvoke(Action action);
    }
}