using System;

namespace Match3
{
    public interface ICellObject : IObject
    {
        event Action<ICell> OwnerChanged;
        
        ICell Owner { get; }

        void SetOwner(ICell owner);

        TCellObjectComponent TryGetComponent<TCellObjectComponent>()
            where TCellObjectComponent : class, ICellObjectComponent;
    }
}