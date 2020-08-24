using System.Collections.Generic;
using UnityEngine;

namespace Match3.View
{
    public class BoardView : MonoBehaviour
    {
        private IBoardObserver _board;

        public GridView _gridViewPrefab;

        private Dictionary<GridId, GridView> _grids = new Dictionary<GridId, GridView>();
        public void Setup(IBoardObserver board, IGameController controller, IViewFactory viewFactory)
        {
            _board = board;
            foreach (var grid in board.Grids)
            {
                GridView gridView = Instantiate(_gridViewPrefab, transform);
                gridView.name = "grid" + grid.Id.Id;
                gridView.Setup(grid, controller, viewFactory);
                _grids.Add(grid.Id, gridView);
            }

            board.CellObjectOwnerChange += OnCellObjectOwnerChange;
            board.CellObjectDestroy += OnCellObjectDestroy;
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
                CellObjectView view = fromGrid.DeAttach(cellObject, oldOwner.Position);
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