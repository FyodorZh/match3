namespace Match3
{
    public interface IGameContext
    {
        Time CurrentTime { get; }
        int GetRandom();
    }
}