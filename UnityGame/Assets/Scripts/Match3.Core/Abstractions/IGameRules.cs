namespace Match3
{
    public interface IGameRules
    {
        IObjectFactory ObjectFactory { get; }
        IViewFactory ViewFactory { get; }
    }
}