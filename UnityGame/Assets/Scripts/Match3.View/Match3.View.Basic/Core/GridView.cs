using Match3.ViewBinding.Default;

namespace Match3.View.Default
{
    public class GridView : GridViewBinding
    {
        public CellView _cellViewPrefab;

        protected override CellViewBinding ConstructCellView()
        {
            CellViewBinding view = Instantiate(_cellViewPrefab, transform);
            return view;
        }

        protected override void OnInit()
        {
            base.OnInit();
            name = "grid" + Grid.Id.Id.ToString();
        }
    }
}