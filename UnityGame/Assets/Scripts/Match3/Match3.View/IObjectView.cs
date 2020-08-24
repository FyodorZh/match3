namespace Match3
{
    public interface IObjectView
    {
        IObjectObserver Owner { get; }
        void Release();
    }
}