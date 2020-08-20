using System;
using System.Collections.Generic;
using Match3;

namespace Match3.Core
{
    class Cell : ICell
    {
        private readonly Grid _owner;

        private readonly List<ICellObject> _objects = new List<ICellObject>();

        private readonly List<ICellComponent> _components = new List<ICellComponent>();

        private bool _isActive;
        private ICell _cellImplementation;

        private readonly LockStack _lockStack = new LockStack();

        public CellId Id { get; }

        public CellPosition Position { get; }

        public IGame Game => _owner.Game;

        public IGrid Owner => _owner;

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                }
            }
        }

        public bool IsLocked => _lockStack.IsLocked;

        public void AddLock(ILock lockObject)
        {
            _lockStack.AddLock(lockObject);
        }

        public void RemoveLock(ILock lockObject)
        {
            _lockStack.RemoveLock(lockObject);
        }

        public Cell(Grid owner, CellPosition position)
        {
            _owner = owner;
            Id = new CellId(owner.Id, position);
            Position = position;
        }

        public void Tick(DeltaTime dTime)
        {
            foreach (var obj in _objects)
            {
                obj.Tick(dTime);
            }

            foreach (var component in _components)
            {
                component.Tick(dTime);
            }
        }

        public IReadOnlyList<ICellObject> Objects => _objects;

        public IReadOnlyList<ICellComponent> Components => _components;

        public void AddComponent<TComponent>(TComponent component)
            where TComponent : class, ICellComponent, ICellComponentInitializer
        {
            Debug.Assert(_components.Find(c => c.GetType() == component.GetType()) == null);
            _components.Add(component);
            component.SetOwner(this);
        }

        public bool CanAttach(ICellObject cellObject)
        {
            if (!IsActive)
                return false;
            if (IsLocked)
                return false;
            foreach (var obj in _objects)
            {
                if (!obj.CanAttachSibling(cellObject))
                    return false;
            }

            return true;
        }

        public bool Attach(ICellObject cellObject)
        {
            if (cellObject == null)
                throw new ArgumentNullException();

            if (ReferenceEquals(cellObject.Owner, this))
                return true;

            if (cellObject.Owner != null)
            {
                var owner = (Cell)cellObject.Owner;

                int count = owner._objects.Count;
                for (int i = 0; i < count; ++i)
                {
                    if (ReferenceEquals(cellObject, owner._objects[i]))
                    {
                        owner._objects.RemoveAt(i);
                        break;
                    }
                }
            }

            var prevOwner = cellObject.Owner;

            _objects.Add(cellObject);
            cellObject.SetOwner(this);

            _owner.Board.OnCellObjectOwnerChange(cellObject, prevOwner);
            return true;
        }

        public bool Destroy(ICellObject cellObject)
        {
            if (cellObject == null)
                throw new ArgumentNullException();

            if (!ReferenceEquals(cellObject.Owner, this))
                throw new InvalidOperationException();

            int count = _objects.Count;
            for (int i = 0; i < count; ++i)
            {
                if (ReferenceEquals(cellObject, _objects[i]))
                {
                    _owner.Board.OnCellObjectDestroy(cellObject);

                    _objects.RemoveAt(i);
                    cellObject.SetOwner(null);
                    cellObject.Release();
                    return true;
                }
            }

            return false;
        }
    }
}