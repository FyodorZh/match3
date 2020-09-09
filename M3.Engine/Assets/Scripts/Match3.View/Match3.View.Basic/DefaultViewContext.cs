using Match3.ViewBinding.Default;

namespace Match3.View.Default
{
    public interface IDefaultViewContext : IViewContext
    {

    }

    public class DefaultViewContext : IDefaultViewContext
    {
        public bool IsEditor { get; }
        public IGameController GameController { get; }

        public IViewFactory Factory { get; }

        public DefaultViewContext(bool isEditor, IGameController controller, IViewFactory factory)
        {
            IsEditor = isEditor;
            GameController = controller;
            Factory = factory;
        }
    }
}