using Match3.ViewBinding.Default;

namespace Match3.View.Default
{
    public class BoardView : BoardViewBinding<IDefaultViewContext>
    {
        public GridView _gridViewPrefab;

        protected override GridViewBinding<IDefaultViewContext> ConstructGridView()
        {
            return Instantiate(_gridViewPrefab, transform);
        }
    }
}