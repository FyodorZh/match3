using UnityEngine;

namespace Match3.ViewBinding.Default
{
    public interface IViewBinding
    {
        IObserver Observer { get; }

        void Init(IObserver observer, IViewContext viewContext);
    }

    public abstract class ViewBinding<TObserver, TViewContext> : MonoBehaviour, IViewBinding
        where TObserver : class, IObserver
        where TViewContext : class, IViewContext
    {
        protected TObserver Observer { get; private set; }

        protected TViewContext ViewContext { get; private set; }

        IObserver IViewBinding.Observer => Observer;

        public void Init(IObserver observer, IViewContext viewContext)
        {
            Observer = (TObserver)observer;
            ViewContext = (TViewContext)viewContext;
            OnInit();
        }

        protected virtual void OnInit()
        {
            // DO NOTHING
        }
    }
}