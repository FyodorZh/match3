using Match3.ViewBinding.Default;

namespace Match3.View.Default
{
    public class BoardView : BoardViewBinding<IDefaultViewContext>
    {
        public CellView _cellViewPrefab;

        protected override CellViewBinding<IDefaultViewContext> ConstructCellView()
        {
            return Instantiate(_cellViewPrefab, transform);
        }

        protected override void OnInit()
        {
            base.OnInit();
            name = "board";
        }
    }
}