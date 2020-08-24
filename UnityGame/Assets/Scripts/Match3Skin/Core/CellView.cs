using System.Collections.Generic;
using UnityEngine;

namespace Match3.View
{
    public class CellView : MonoBehaviour
    {
        private ICellObserver _cell;
        private IViewFactory _viewFactory;

        private readonly List<CellObjectView> _objects = new List<CellObjectView>();

        public GameObject _cellViewActive;
        public GameObject _cellViewInactive;

        public GameObject _cellViewLock;

        public CellTouch _cellTouch;


        public void Setup(ICellObserver cell, IGameController controller, IViewFactory viewFactory)
        {
            _cell = cell;
            _viewFactory = viewFactory;

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

        public void Add(ICellObjectObserver obj)
        {
            var view = _viewFactory.Construct<CellObjectView>(obj);
            view.name = obj.TypeId.Id;

            Attach(view, false);
        }

        public void Attach(CellObjectView cellObjectView, bool preserveWorldPosition)
        {
            _objects.Add(cellObjectView);
            cellObjectView.transform.SetParent(transform, preserveWorldPosition);
        }

        public CellObjectView DeAttach(ICellObjectObserver cellObject)
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

        public void Destroy(ICellObjectObserver cellObject)
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