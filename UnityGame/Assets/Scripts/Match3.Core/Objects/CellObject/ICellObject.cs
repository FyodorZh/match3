using System;
using Match3.Math;

namespace Match3
{
    public interface ICellObject : IObject
    {
        ICell Owner { get; }

        void SetOwner(ICell owner);

        TCellObjectComponent TryGetComponent<TCellObjectComponent>()
            where TCellObjectComponent : class, ICellObjectComponent;

        void Tick(Fixed dTimeSeconds);
    }
}