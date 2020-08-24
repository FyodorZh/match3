namespace Match3
{
    public interface IObjectComponentObserver
    {
        string TypeId { get; }
        bool IsReleased { get; }
    }
}