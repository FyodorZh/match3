namespace Match3
{
    public interface ICellObject : IObject, ICellObjectObserver
    {
        new ICell Owner { get; }

        void SetOwner(ICell owner);

        bool CanAttachSibling(ICellObject sibling);

        new TCellObjectComponent TryGetComponent<TCellObjectComponent>()
            where TCellObjectComponent : class, ICellObjectComponent;

        void Tick(DeltaTime dTime);
    }
}