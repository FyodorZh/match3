namespace Match3
{
    public interface IGame
    {
        IGameRules Rules { get; }
        IBoard Board { get; }
        void Tick(int dTimeMs);
    }
}