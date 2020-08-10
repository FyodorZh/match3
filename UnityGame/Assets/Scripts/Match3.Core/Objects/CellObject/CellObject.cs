using System;
using System.Collections.Generic;
using System.Linq;

namespace Match3
{
    public class CellObject : Match3.Object, ICellObject
    {
        private readonly ICellObjectComponent[] _components;
        
        public CellObject(ObjectTypeId type, IEnumerable<ICellObjectComponent> components) 
            : this(type, components.ToArray())
        {
            _components = components.ToArray();
        }
        
        public CellObject(ObjectTypeId type, params ICellObjectComponent[] components) 
            : base(type)
        {
            _components = components.ToArray();
        }

        protected override void OnRelease()
        {
            SetOwner(null);
        }

        public event Action<ICell> OwnerChanged;
        
        public ICell Owner { get; private set; }

        public void SetOwner(ICell owner)
        {
            Owner = owner;
            OwnerChanged?.Invoke(owner);
        }

        public TCellObjectComponent TryGetComponent<TCellObjectComponent>() 
            where TCellObjectComponent : class, ICellObjectComponent
        {
            foreach (var component in _components)
            {
                if (component is TCellObjectComponent typedComponent)
                {
                    return typedComponent;
                }
            }

            return default;
        }
    }
}