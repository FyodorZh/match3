namespace Match3
{
    public interface IGame : IGameContext
    {
        IGameRules Rules { get; }
        IBoard Board { get; }
        void Tick(int dTimeMs);
    }
}