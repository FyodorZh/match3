using UnityEngine;

namespace Match3.ViewBinding.Default
{
    public abstract class GameViewBinding : MonoBehaviour
    {
        private IGameObserver _game;
        private IGameController _controller;

        private BoardViewBinding _boardView;

        protected abstract BoardViewBinding ConstructBoardView();

        public void Init(IGameObserver game, IGameController controller, IViewFactory viewFactory)
        {
            _game = game;
            _controller = controller;

            _boardView = ConstructBoardView();
            _boardView.Init(_game.Board, _controller, viewFactory);

            OnInit();
        }

        protected virtual void OnInit()
        {
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