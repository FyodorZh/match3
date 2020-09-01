using System.Collections.Generic;
using UnityEngine;

namespace Match3.ViewBinding.Default
{
    public abstract class CellViewBinding : MonoBehaviour
    {
        protected ICellObserver Cell { get; private set; }

        protected IGameController GameController { get; private set; }

        private IViewFactory _viewFactory;

        private readonly List<CellObjectViewBinding> _objects = new List<CellObjectViewBinding>();

        public void Init(ICellObserver cell, IGameController controller, IViewFactory viewFactory)
        {
            Cell = cell;
            _viewFactory = viewFactory;

            GameController = controller;

            foreach (var obj in cell.Objects)
            {
                Add(obj);
            }

            OnInit();
        }

        protected virtual void OnInit()
        {
        }

        protected virtual void Update()
        {
        }

        public void Add(ICellObjectObserver obj)
        {
            var view = _viewFactory.Construct<CellObjectViewBinding>(obj);
            view.name = obj.TypeId.Id;

            Attach(view, false);
        }

        public void Attach(CellObjectViewBinding cellObjectView, bool preserveWorldPosition)
        {
            _objects.Add(cellObjectView);
            cellObjectView.transform.SetParent(transform, preserveWorldPosition);
        }

        public CellObjectViewBinding DeAttach(ICellObjectObserver cellObject)
        {
            int count = _objects.Count;
            for (int i = 0; i < count; ++i)
            {
                if (cellObject == _objects[i].Owner)
                {
                    CellObjectViewBinding view = _objects[i];
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
                    CellObjectViewBinding view = _objects[i];
                    _objects.RemoveAt(i);
                    Destroy(view.gameObject);
                    return;
                }
            }
        }
    }
}