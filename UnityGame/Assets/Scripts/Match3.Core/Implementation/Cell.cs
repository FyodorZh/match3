using System;
using System.Collections.Generic;

namespace Match3.Core
{
    class Cell : ICell
    {
        private readonly Grid _owner;

        private readonly List<ICellObject> _content = new List<ICellObject>();

        private bool _isActive;

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

        public IReadOnlyList<ICellObject> Content => _content;

        public bool TryAddContent(ICellObject cellObject)
        {
            if (cellObject == null)
            {
                return false;
            }

            if (!IsActive)
            {
                return false;
            }
            
            string typeId = cellObject.TypeId.Id;
            foreach (var obj in _content)
            {
                if (typeId == obj.TypeId.Id)
                {
                    return false;
                }
            }
            
            _content.Add(cellObject);
            ContentAdded?.Invoke(cellObject);
            return true;
        }
    }
}