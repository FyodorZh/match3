using UnityEngine;

namespace Match3.ViewBinding.Default
{
    public abstract class GridViewBinding<TViewContext> : ViewBinding<IGridObserver, TViewContext>
        where TViewContext : class, IViewContext
    {
        private CellViewBinding<TViewContext>[,] _cells;

        protected abstract CellViewBinding<TViewContext> ConstructCellView();

        protected override void OnInit()
        {
            base.OnInit();

            _cells = new CellViewBinding<TViewContext>[Observer.Width, Observer.Height];
            foreach (var cell in Observer.AllCells)
            {
                CellViewBinding<TViewContext> view = ConstructCellView();
                view.transform.position = new Vector3(cell.Position.X, 0, cell.Position.Y);
                view.Init(cell, ViewContext);
                _cells[cell.Position.X, cell.Position.Y] = view;
            }
        }

        public void Add(ICellObjectObserver cellObject, CellPosition position)
        {
            var cellView = _cells[position.X, position.Y];
            cellView.Add(cellObject);
        }

        public void Attach(ICellObjectViewBinding cellObjectView, CellPosition position)
        {
            var cellView = _cells[position.X, position.Y];
            cellView.Attach(cellObjectView, true);
        }

        public ICellObjectViewBinding DeAttach(ICellObjectObserver cellObject, CellPosition oldPosition)
        {
            var oldCell = _cells[oldPosition.X, oldPosition.Y];
            return oldCell.DeAttach(cellObject);
        }

        public void Destroy(ICellObjectObserver cellObject)
        {
            var cellView = _cells[cellObject.Owner.Position.X, cellObject.Owner.Position.Y];
            cellView.Destroy(cellObject);
        }
    }
}