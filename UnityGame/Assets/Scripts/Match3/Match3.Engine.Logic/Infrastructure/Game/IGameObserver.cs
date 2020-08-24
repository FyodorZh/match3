namespace Match3
{
    public interface IGameObserver
    {
        IBoardObserver Board { get; }
    }
}