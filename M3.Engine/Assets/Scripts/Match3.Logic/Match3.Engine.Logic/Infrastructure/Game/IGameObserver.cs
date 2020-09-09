namespace Match3
{
    public interface IGameObserver : IObserver
    {
        IBoardObserver Board { get; }
    }
}