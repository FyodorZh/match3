namespace Match3.ViewBinding.Default
{
    public interface ICellObjectViewBinding : IObjectViewBinding
    {
    }

    public abstract class CellObjectViewBinding<TObserver, TViewContext> : ObjectViewBinding<TObserver, TViewContext>, ICellObjectViewBinding
        where TObserver : class, ICellObjectObserver
        where TViewContext : class, IViewContext
    {
    }
}