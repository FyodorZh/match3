namespace Match3
{
    public interface ICellObject : IObject
    {
        TCellObjectComponent TryGetComponent<TCellObjectComponent>()
            where TCellObjectComponent : class, ICellObjectComponent;
    }
}