using Match3.ViewBinding.Default;

namespace Match3.View.Default
{
    public class BoardView : BoardViewBinding
    {
        public GridView _gridViewPrefab;

        protected override GridViewBinding ConstructGridView()
        {
            return Instantiate(_gridViewPrefab, transform);
        }
    }
}