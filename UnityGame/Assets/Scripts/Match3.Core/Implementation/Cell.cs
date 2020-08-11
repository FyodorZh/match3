using System;
using System.Collections.Generic;
using Match3.Math;

namespace Match3.Core
{
    class Cell : ICell
    {
        private readonly Grid _owner;

        private readonly List<ICellObject> _objects = new List<ICellObject>();

        private bool _isActive;
        private ICell _cellImplementation;

        private readonly List<object> _lockObjects = new List<object>();
        
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

        public bool IsLocked => _lockObjects.Count > 0;
        public void AddLock(object lockObject)
        {
            Debug.Assert(!_lockObjects.Contains(lockObject));
            _lockObjects.Add(lockObject);
        }

        public void RemoveLock(object lockObject)
        {
            _lockObjects.Remove(lockObject);
        }

        public Cell(Grid owner, CellPosition position)
        {
            _owner = owner;
            Id = new CellId(owner.Id, position);
            Position = position;
        }
        
        public void Tick(Fixed dTimeSeconds)
        {
            foreach (var obj in _objects)
            {
                obj.Tick(dTimeSeconds);
            }
        }

        public IReadOnlyList<ICellObject> Objects => _objects;
        
        public bool CanAttach(ICellObject cellObject)
        {
            return IsActive && !IsLocked;
        }

        public bool Attach(ICellObject cellObject)
        {
            if (cellObject == null)
                throw new ArgumentNullException();
            
            if (ReferenceEquals(cellObject.Owner, this))
                return true;

            if (!CanAttach(cellObject))
                return false;
            
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