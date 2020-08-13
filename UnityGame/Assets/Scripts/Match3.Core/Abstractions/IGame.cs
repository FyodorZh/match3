using System;

namespace Match3
{
    public interface IGameController
    {
        void Start();

        void Tick(int dTimeMs);

        void Action(string actionFeatureName, params CellId[] cells);
    }

    public interface IGame : IGameContext
    {
        IGameRules Rules { get; }
        IBoard Board { get; }

        void InternalInvoke(Action action);
    }
}