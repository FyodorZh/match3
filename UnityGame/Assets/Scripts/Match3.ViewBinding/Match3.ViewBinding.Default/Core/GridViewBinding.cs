using UnityEngine;

namespace Match3.ViewBinding.Default
{
    public abstract class GridViewBinding : MonoBehaviour
    {
        protected IGridObserver Grid { get; private set; }

        private CellViewBinding[,] _cells;

        protected abstract CellViewBinding ConstructCellView();

        public void Init(IGridObserver grid, IGameController controller, IViewFactory viewFactory)
        {
            Grid = grid;

            _cells = new CellViewBinding[grid.Width, grid.Height];
            foreach (var cell in grid.AllCells)
            {
                CellViewBinding view = ConstructCellView();
                view.transform.position = new Vector3(cell.Position.X, 0, cell.Position.Y);
                view.Init(cell, controller, viewFactory);
                _cells[cell.Position.X, cell.Position.Y] = view;
            }

            OnInit();
        }

        protected virtual void OnInit()
        {
        }

        public void Add(ICellObjectObserver cellObject, CellPosition position)
        {
            CellViewBinding cellView = _cells[position.X, position.Y];
            cellView.Add(cellObject);
        }

        public void Attach(CellObjectViewBinding cellObjectView, CellPosition position)
        {
            CellViewBinding cellView = _cells[position.X, position.Y];
            cellView.Attach(cellObjectView, true);
        }

        public CellObjectViewBinding DeAttach(ICellObjectObserver cellObject, CellPosition oldPosition)
        {
            CellViewBinding oldCell = _cells[oldPosition.X, oldPosition.Y];
            return oldCell.DeAttach(cellObject);
        }

        public void Destroy(ICellObjectObserver cellObject)
        {
            CellViewBinding cellView = _cells[cellObject.Owner.Position.X, cellObject.Owner.Position.Y];
            cellView.Destroy(cellObject);
        }
    }
}