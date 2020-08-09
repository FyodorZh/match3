namespace Match3
{
    public interface ICellObject : IObject
    {
        ICellObjectType Type { get; }
    }
}