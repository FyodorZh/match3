namespace Match3.ViewBinding.Default
{
    public interface IViewContext
    {
        bool IsEditor { get; }

        IGameController GameController { get; }

        IViewFactory Factory { get; }
    }
}