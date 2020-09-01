namespace Match3
{
    public interface ICellObjectObserver : IObjectObserver
    {
        ICellObserver Owner { get; }

        TCellObjectComponent TryGetComponent<TCellObjectComponent>()
            where TCellObjectComponent : class, ICellObjectComponentObserver;
    }
}