﻿using System;
using System.Collections.Generic;
using Match3.Utils;

namespace Match3.Logic
{
    class Cell : ICell
    {
        private readonly Board _owner;

        private readonly StaticSet<ICellObject> _objects = new StaticSet<ICellObject>();

        private readonly List<ICellComponent> _components = new List<ICellComponent>();

        private bool _isActive;
        private ICell _cellImplementation;

        private readonly LockStack _lockStack = new LockStack();

        public CellPosition Position { get; }

        public IGame Game => _owner.Game;
        IGameObserver ICellObserver.Game => _owner.Game;

        public IBoard Owner => _owner;
        IBoardObserver ICellObserver.Owner => _owner;

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

        public Cell(Board owner, CellPosition position)
        {
            _owner = owner;
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

        public IStaticSetView<ICellObject> Objects => _objects;
        IEnumerable<ICellObjectObserver> ICellObserver.Objects => _objects;

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
            if (ReferenceEquals(this, cellObject.Owner))
                return true;

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

        public void Attach(ICellObject cellObject)
        {
            if (cellObject == null)
                throw new ArgumentNullException();

            if (ReferenceEquals(cellObject.Owner, this))
                return;

            if (cellObject.Owner != null)
            {
                var owner = (Cell)cellObject.Owner;
                owner._objects.Remove(cellObject.GetType());
            }

            var prevOwner = cellObject.Owner;

            var oldObject = _objects.Replace(cellObject);
            Debug.Assert(oldObject == null);
            cellObject.SetOwner(this);

            _owner.OnCellObjectOwnerChange(cellObject, prevOwner);
        }

        public bool CanSwap(ICellObject cellObjectA, ICellObject cellObjectB)
        {
            if (ReferenceEquals(cellObjectA.Owner, cellObjectB.Owner))
                return true;

            var ownerA = (Cell)cellObjectA.Owner;
            var ownerB = (Cell)cellObjectB.Owner;

            if (ownerA == null || ownerB == null)
                return false;

            if (cellObjectA.GetType() != cellObjectB.GetType())
            {
                return ownerA.CanAttach(cellObjectB) && ownerB.CanAttach(cellObjectA);
            }

            var objA = ownerA._objects.Remove(cellObjectA.GetType());
            var objB = ownerB._objects.Remove(cellObjectB.GetType());

            Debug.Assert(ReferenceEquals(objA, cellObjectA));
            Debug.Assert(ReferenceEquals(objB, cellObjectB));

            bool okA = ownerA.CanAttach(cellObjectB);
            bool okB = ownerB.CanAttach(cellObjectA);

            bool ok = okA && okB;

            ownerA._objects.Replace(objA);
            ownerB._objects.Replace(objB);

            return ok;
        }

        public void Swap(ICellObject cellObjectA, ICellObject cellObjectB)
        {
            if (ReferenceEquals(cellObjectA.Owner, cellObjectB.Owner))
                return;

            var ownerA = (Cell)cellObjectA.Owner;
            var ownerB = (Cell)cellObjectB.Owner;

            if (ownerA == null || ownerB == null)
                return;

            if (cellObjectA.GetType() != cellObjectB.GetType())
            {
                ownerA.Attach(cellObjectB);
                ownerB.Attach(cellObjectA);
                return;
            }

            var objA = ownerA._objects.Remove(cellObjectA.GetType());
            var objB = ownerB._objects.Remove(cellObjectB.GetType());

            Debug.Assert(ReferenceEquals(objA, cellObjectA));
            Debug.Assert(ReferenceEquals(objB, cellObjectB));

            ownerA._objects.Replace(objB);
            ownerB._objects.Replace(objA);

            objA.SetOwner(ownerB);
            objB.SetOwner(ownerA);

            _owner.OnCellObjectOwnerChange(objA, ownerA);
            _owner.OnCellObjectOwnerChange(objB, ownerB);
        }

        public bool Destroy(ICellObject cellObject)
        {
            if (cellObject == null)
                throw new ArgumentNullException();

            if (!ReferenceEquals(cellObject.Owner, this))
                throw new InvalidOperationException();

            var removed = _objects.Remove(cellObject.GetType());
            Debug.Assert(ReferenceEquals(removed, cellObject));

            _owner.OnCellObjectDestroy(cellObject);
            cellObject.SetOwner(null);
            cellObject.Release();
            return true;
        }
    }
}