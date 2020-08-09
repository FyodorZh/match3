using System.Collections;
using System.Collections.Generic;
using Match3;
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
            _game.Tick((int)(Time.deltaTime * 1000));
        }
    }
}