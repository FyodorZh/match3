using UnityEngine;

namespace Match3.View
{
    public class GameView : MonoBehaviour
    {
        private IGame _game;
        private IGameController _controller;

        public BoardView _boardViewPrefab;

        private BoardView _boardView;

        public void Setup(IGame game, IGameController controller)
        {
            _game = game;
            _controller = controller;

            _boardView = Instantiate(_boardViewPrefab, transform);
            _boardView.name = "Board";
            _boardView.Setup(game.Board, controller);
        }

        void Update()
        {
            int dt = (int)(Time.deltaTime * 1000);
            if (dt <= 0)
                dt = 1;
            _controller.Tick(dt);
        }
    }
}