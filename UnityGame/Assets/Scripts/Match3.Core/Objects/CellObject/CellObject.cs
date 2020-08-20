using System.Collections.Generic;
using System.Linq;
using Match3;

namespace Match3.Core
{
    public class CellObject : Core.Object, ICellObject
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

        protected virtual void OnChangeOwner(ICell newOwner)
        {
        }

        protected virtual void OnTick(DeltaTime dTime)
        {
        }

        protected override void OnRelease()
        {
            Owner?.Destroy(this);

            SetOwner(null);
            foreach (var component in _components)
            {
                component.Release();
            }
        }

        public ICell Owner { get; private set; }

        public void SetOwner(ICell owner)
        {
            OnChangeOwner(owner);
            Owner = owner;
        }

        public virtual bool CanAttachSibling(ICellObject sibling)
        {
            return true;
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

        public void Tick(DeltaTime dTime)
        {
            foreach (var component in _components)
            {
                component.Tick(dTime);
            }
            OnTick(dTime);
        }
    }
}