using UnityEngine;

namespace Match3.ViewBinding.Default
{
    public interface IObjectViewBinding : IViewBinding
    {
        GameObject RootGO { get; }

        void Release();
    }

    public abstract class ObjectViewBinding<TObserver, TViewContext> : ViewBinding<TObserver, TViewContext>, IObjectViewBinding
        where TObserver : class, IObjectObserver
        where TViewContext : class, IViewContext
    {
        public GameObject RootGO => gameObject;

        public void Release()
        {
            Destroy(this);
        }

        protected virtual void Update()
        {
        }
    }
}