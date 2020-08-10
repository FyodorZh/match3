namespace Match3
{
    public interface ICellObject : IObject
    {
        TCellObjectComponent TryGetFeature<TCellObjectComponent>()
            where TCellObjectComponent : ICellObjectComponent;
    }
}