using System;
using System.Collections.Generic;
using System.Linq;
using Match3.Math;

namespace Match3
{
    public class CellObject : Match3.Object, ICellObject
    {
        private readonly ICellObjectComponent[] _components;
        
        public CellObject(ObjectTypeId type, IEnumerable<ICellObjectComponent> components) 
            : this(type, components.ToArray())
        {
        }
        
        public CellObject(ObjectTypeId type, params ICellObjectComponent[] components) 
            : base(type)
        {
            _components = components;
            foreach (var component in _components)
            {
                component.SetOwner(this);
            }
        }

        protected override void OnRelease()
        {
            SetOwner(null);
            foreach (var component in _components)
            {
                component.Release();
            }
        }
        
        public ICell Owner { get; private set; }

        public void SetOwner(ICell owner)
        {
            Owner = owner;
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
        
        public void Tick(Fixed dTimeSeconds)
        {
            foreach (var component in _components)
            {
                component.Tick(dTimeSeconds);
            }
        }
    }
}