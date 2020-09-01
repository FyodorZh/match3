namespace Match3
{
    public interface IBorderObserver
    {
        BorderId Id { get; }
        BorderPosition Position { get; }
    }
}