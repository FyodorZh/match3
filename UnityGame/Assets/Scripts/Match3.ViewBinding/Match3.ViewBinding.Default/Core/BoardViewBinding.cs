using System.Collections.Generic;
using UnityEngine;

namespace Match3.ViewBinding.Default
{
    public abstract class BoardViewBinding<TViewContext> : ViewBinding<IBoardObserver, TViewContext>
        where TViewContext : class, IViewContext
    {
        private readonly Dictionary<GridId, GridViewBinding<TViewContext>> _grids =
            new Dictionary<GridId, GridViewBinding<TViewContext>>();

        protected abstract GridViewBinding<TViewContext> ConstructGridView();

        protected override void OnInit()
        {
            base.OnInit();

            foreach (var grid in Observer.Grids)
            {
                var gridView = ConstructGridView();
                gridView.Init(grid, ViewContext);
                _grids.Add(grid.Id, gridView);
            }

            Observer.CellObjectOwnerChange += OnCellObjectOwnerChange;
            Observer.CellObjectDestroy += OnCellObjectDestroy;
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
                var view = fromGrid.DeAttach(cellObject, oldOwner.Position);
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