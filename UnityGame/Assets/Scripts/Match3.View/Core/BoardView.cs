using System.Collections.Generic;
using UnityEngine;

namespace Match3.View
{
    public class BoardView : MonoBehaviour
    {
        private IBoard _board;
        
        public GridView _gridViewPrefab;

        private Dictionary<GridId, GridView> _grids = new Dictionary<GridId, GridView>();
        public void Setup(IBoard board)
        {
            _board = board;
            foreach (var grid in board.Grids)
            {
                GridView gridView = Instantiate(_gridViewPrefab, transform);
                gridView.name = "grid" + grid.Id.Id;
                gridView.Setup(grid);
                _grids.Add(grid.Id, gridView);
            }

            board.CellObjectOwnerChange += OnCellObjectOwnerChange;
        }
        
        private void OnCellObjectOwnerChange(ICellObject cellObject, ICell oldOwner)
        {
            string to = (cellObject.Owner != null) ? cellObject.Owner.Id.ToString() : "null";
            string from = oldOwner != null ? oldOwner.Id.ToString() : "null";
            Debug.Log($"Move {from} -> {to}");

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
    }
}