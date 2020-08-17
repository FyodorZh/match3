namespace Match3
{
    public interface IGameContext
    {
        int CurrentTime { get; }
        int GetRandom();
    }
}