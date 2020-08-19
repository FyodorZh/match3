using Match3;

namespace Match3
{
    public interface ICellObject : IObject
    {
        ICell Owner { get; }

        void SetOwner(ICell owner);

        bool CanAttachSibling(ICellObject sibling);

        TCellObjectComponent TryGetComponent<TCellObjectComponent>()
            where TCellObjectComponent : class, ICellObjectComponent;

        void Tick(Fixed dTimeSeconds);
    }
}