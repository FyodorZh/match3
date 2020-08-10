namespace Match3
{
    public interface ICellObjectComponent : IObjectComponent
    {
        ICellObject Owner { get; }
        void SetOwner(ICellObject owner);
    }
}