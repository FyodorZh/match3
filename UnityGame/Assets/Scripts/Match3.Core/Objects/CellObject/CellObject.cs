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
            // DO NOTHING
        }

        public TCellObjectComponent TryGetComponent<TCellObjectComponent>(string componentTypeId) 
            where TCellObjectComponent : class, ICellObjectComponent
        {
            foreach (var component in _components)
            {
                if (component.TypeId == componentTypeId)
                    return component as TCellObjectComponent;
            }

            return default;
        }
    }
}