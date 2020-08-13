using System.Collections.Generic;
using UnityEngine;

namespace Match3.View
{
    public class CellView : MonoBehaviour
    {
        private ICell _cell;
        private IGameRules _rules;

        private readonly List<CellObjectView> _objects = new List<CellObjectView>();

        public GameObject _cellViewActive;
        public GameObject _cellViewInactive;

        public GameObject _cellViewLock;

        public CellTouch _cellTouch;


        public void Setup(ICell cell, IGameController controller)
        {
            _cell = cell;
            _rules = cell.Game.Rules;

            foreach (var obj in cell.Objects)
            {
                Add(obj);
            }

            _cellViewActive.SetActive(_cell.IsActive);
            _cellViewInactive.SetActive(!_cell.IsActive);

            _cellTouch.Setup(cell, controller);
        }

        private void Update()
        {
            _cellViewLock.SetActive(_cell.IsLocked);
        }

        public void Add(ICellObject obj)
        {
            var view = _rules.ViewFactory.Construct<CellObjectView>(obj);
            view.name = obj.TypeId.Id;

            Attach(view, false);
        }

        public void Attach(CellObjectView cellObjectView, bool preserveWorldPosition)
        {
            _objects.Add(cellObjectView);
            cellObjectView.transform.SetParent(transform, preserveWorldPosition);
        }

        public CellObjectView DeAttach(ICellObject cellObject)
        {
            int count = _objects.Count;
            for (int i = 0; i < count; ++i)
            {
                if (cellObject == _objects[i].Owner)
                {
                    CellObjectView view = _objects[i];
                    _objects.RemoveAt(i);
                    return view;
                }
            }

            return null;
        }

        public void Destroy(ICellObject cellObject)
        {
            int count = _objects.Count;
            for (int i = 0; i < count; ++i)
            {
                if (cellObject == _objects[i].Owner)
                {
                    CellObjectView view = _objects[i];
                    _objects.RemoveAt(i);
                    Destroy(view.gameObject);
                    return;
                }
            }
        }
    }
}