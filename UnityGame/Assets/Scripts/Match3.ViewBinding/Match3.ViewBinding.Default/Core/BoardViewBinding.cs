using System.Collections.Generic;
using UnityEngine;

namespace Match3.ViewBinding.Default
{
    public abstract class BoardViewBinding : MonoBehaviour
    {
        protected IBoardObserver Board { get; private set; }

        private readonly Dictionary<GridId, GridViewBinding> _grids = new Dictionary<GridId, GridViewBinding>();

        protected abstract GridViewBinding ConstructGridView();

        public void Init(IBoardObserver board, IGameController controller, IViewFactory viewFactory)
        {
            Board = board;
            foreach (var grid in board.Grids)
            {
                GridViewBinding gridView = ConstructGridView();
                gridView.Init(grid, controller, viewFactory);
                _grids.Add(grid.Id, gridView);
            }

            board.CellObjectOwnerChange += OnCellObjectOwnerChange;
            board.CellObjectDestroy += OnCellObjectDestroy;

            OnInit();
        }

        protected virtual void OnInit()
        {
        }

        private void OnCellObjectOwnerChange(ICellObjectObserver cellObject, ICellObserver oldOwner)
        {
            //string to = (cellObject.Owner != null) ? cellObject.Owner.Id.ToString() : "null";
            //string from = oldOwner != null ? oldOwner.Id.ToString() : "null";
            //Debug.Log($"Move {from} -> {to}");

            var fromGrid = oldOwner != null ? _grids[oldOwner.Id.GridId] : null;
            var toGrid = _grids[cellObject.Owner.Id.GridId];

            if (fromGrid != null)
            {
                CellObjectViewBinding view = fromGrid.DeAttach(cellObject, oldOwner.Position);
                toGrid.Attach(view, cellObject.Owner.Position);
            }
            else
            {
                toGrid.Add(cellObject, cellObject.Owner.Position);
            }
        }

        private void OnCellObjectDestroy(ICellObjectObserver cellObject)
        {
            var grid = _grids[cellObject.Owner.Id.GridId];
            grid.Destroy(cellObject);
        }
    }
}