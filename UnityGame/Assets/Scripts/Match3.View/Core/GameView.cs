using UnityEngine;

namespace Match3.View
{
    public class GameView : MonoBehaviour
    {
        private IGame _game;

        public BoardView _boardViewPrefab;

        private BoardView _boardView;

        public void Setup(IGame game)
        {
            _game = game;
            _boardView = Instantiate(_boardViewPrefab, transform);
            _boardView.name = "Board";
            _boardView.Setup(game.Board);
        }

        void Update()
        {
            int dt = (int)(Time.deltaTime * 1000);
            if (dt <= 0)
                dt = 1;
            _game.Tick(dt);
        }
    }
}