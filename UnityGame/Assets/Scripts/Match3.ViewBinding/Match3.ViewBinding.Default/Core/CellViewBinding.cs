using System.Collections.Generic;

namespace Match3.ViewBinding.Default
{
    public abstract class CellViewBinding<TViewContext> : ViewBinding<ICellObserver, TViewContext>
        where TViewContext : class, IViewContext
    {
        private readonly List<ICellObjectViewBinding> _objects = new List<ICellObjectViewBinding>();

        protected override void OnInit()
        {
            base.OnInit();
            foreach (var obj in Observer.Objects)
            {
                Add(obj);
            }
        }

        protected virtual void Update()
        {
        }

        public void Add(ICellObjectObserver obj)
        {
            var view = ViewContext.Factory.Construct<ICellObjectViewBinding>(obj.TypeId);
            Debug.Assert(view != null);
            if (view != null)
            {
                view.Init(obj, ViewContext);
                view.RootGO.name = obj.TypeId.Id;

                Attach(view, false);
            }
        }

        public void Attach(ICellObjectViewBinding cellObjectView, bool preserveWorldPosition)
        {
            _objects.Add(cellObjectView);
            cellObjectView.RootGO.transform.SetParent(transform, preserveWorldPosition);
        }

        public ICellObjectViewBinding DeAttach(ICellObjectObserver cellObject)
        {
            int count = _objects.Count;
            for (int i = 0; i < count; ++i)
            {
                if (cellObject == _objects[i].Observer)
                {
                    var view = _objects[i];
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
                if (cellObject == _objects[i].Observer)
                {
                    var view = _objects[i];
                    _objects.RemoveAt(i);
                    Destroy(view.RootGO);
                    return;
                }
            }
        }
    }
}