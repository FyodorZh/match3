using System;
using System.Collections.Generic;
using System.Text;

namespace Match3.Core
{
    class Cell : ICell
    {
        private readonly Grid _owner;

        private readonly List<ICellObject> _objects = new List<ICellObject>();

        private bool _isActive;
        private ICell _cellImplementation;

        public event Action<ICellObject> ContentAdded;
        
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

        public Cell(Grid owner, CellPosition position)
        {
            _owner = owner;
            Id = new CellId(owner.Id, position);
            Position = position;
        }

        public IReadOnlyList<ICellObject> Objects => _objects;

        public bool AddObject(ICellObject cellObject)
        {
            if (AttachObject(cellObject))
            {
                ContentAdded?.Invoke(cellObject);
                return true;
            }
            else
            {
                Debug.Assert(false);
                cellObject.Release();
                return false;
            }
        }

        public bool AttachObject(ICellObject cellObject)
        {
            if (cellObject == null)
            {
                throw new ArgumentNullException(nameof(cellObject));
            }
            
            Debug.Assert(IsActive);
            
            string typeId = cellObject.TypeId.Id;
            foreach (var obj in _objects)
            {
                if (typeId == obj.TypeId.Id)
                {
                    return false;
                }
            }
            _objects.Add(cellObject);
            cellObject.SetOwner(this);
            return true;
        }

        public bool DeattachObject(ICellObject cellObject)
        {
            if (cellObject == null)
            {
                throw new ArgumentNullException(nameof(cellObject));
            }
            
            string typeId = cellObject.TypeId.Id;
            int count = _objects.Count;
            for (int i = 0; i < count; ++i)
            {
                if (typeId == _objects[i].TypeId.Id)
                {
                    _objects.RemoveAt(i);
                    cellObject.SetOwner(null);
                    return true;
                }
            }

            return false;
        }

        public void RemoveObject(ICellObject cellObject)
        {
            if (DeattachObject(cellObject))
            {
                cellObject.Release();
                return;
            }
            Debug.Assert(false);
        }
    }
}