namespace Match3
{
    public interface IObjectObserver : IObserver
    {
        ObjectTypeId TypeId { get; }
    }
}