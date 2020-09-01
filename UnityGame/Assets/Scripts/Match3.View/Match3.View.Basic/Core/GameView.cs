using Match3.ViewBinding.Default;

namespace Match3.View.Default
{
    public class GameView : GameViewBinding
    {
        public BoardView _boardViewPrefab;

        protected override BoardViewBinding ConstructBoardView()
        {
            var boardView = Instantiate(_boardViewPrefab, transform);
            boardView.name = "Board";
            return boardView;
        }
    }
}