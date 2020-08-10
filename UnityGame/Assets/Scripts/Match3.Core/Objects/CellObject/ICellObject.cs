namespace Match3
{
    public interface ICellObject : IObject
    {
        TCellObjectComponent TryGetComponent<TCellObjectComponent>(string componentTypeId)
            where TCellObjectComponent : class, ICellObjectComponent;
    }
}