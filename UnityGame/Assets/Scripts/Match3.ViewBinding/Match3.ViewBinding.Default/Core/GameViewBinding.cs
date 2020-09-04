using UnityEngine;

namespace Match3.ViewBinding.Default
{
    public abstract class GameViewBinding<TViewContext> : ViewBinding<IGameObserver, TViewContext>
        where TViewContext : class, IViewContext
    {
        private IGameController _controller;

        private BoardViewBinding<TViewContext> _boardView;

        protected abstract BoardViewBinding<TViewContext> ConstructBoardView();

        protected override void OnInit()
        {
            base.OnInit();

            _controller = ViewContext.GameController;

            _boardView = ConstructBoardView();
            _boardView.Init(Observer.Board, ViewContext);
        }

        protected virtual void Update()
        {
            int dt = (int)(UnityEngine.Time.deltaTime * 1000);
            if (dt <= 0)
                dt = 1;
            _controller.Tick(new DeltaTime(dt));
        }
    }
}