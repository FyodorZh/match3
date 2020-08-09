namespace Match3
{
    public interface ICellObject : IObject
    {
        TCellObjectFeature TryGetFeature<TCellObjectFeature>()
            where TCellObjectFeature : ICellObjectFeature;
    }
}