using UnityEngine;

namespace Match3.View
{
    public class GameView : MonoBehaviour
    {
        private IGameObserver _game;
        private IGameController _controller;

        public BoardView _boardViewPrefab;

        private BoardView _boardView;

        public void Setup(IGameObserver game, IGameController controller, IViewFactory viewFactory)
        {
            _game = game;
            _controller = controller;

            _boardView = Instantiate(_boardViewPrefab, transform);
            _boardView.name = "Board";
            _boardView.Setup(game.Board, controller, viewFactory);
        }

        void Update()
        {
            int dt = (int)(UnityEngine.Time.deltaTime * 1000);
            if (dt <= 0)
                dt = 1;
            _controller.Tick(new DeltaTime(dt));
        }
    }
}