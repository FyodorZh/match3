using UnityEngine;

namespace Match3.View
{
    public class GridView : MonoBehaviour
    {
        private IGrid _grid;

        public CellView _cellViewPrefab;

        private CellView[,] _cells;

        public void Setup(IGrid grid, IGameController controller)
        {
            _grid = grid;

            _cells = new CellView[grid.Width, grid.Height];
            foreach (var cell in grid.AllCells)
            {
                CellView view = Instantiate(_cellViewPrefab, transform);
                view.name = "cell." + cell.Position.X + "x" + cell.Position.Y;
                view.transform.position = new Vector3(cell.Position.X, 0, cell.Position.Y);
                view.Setup(cell, controller);
                _cells[cell.Position.X, cell.Position.Y] = view;
            }
        }

        public void Add(ICellObject cellObject, CellPosition position)
        {
            CellView cellView = _cells[position.X, position.Y];
            cellView.Add(cellObject);
        }

        public void Attach(CellObjectView cellObjectView, CellPosition position)
        {
            CellView cellView = _cells[position.X, position.Y];
            cellView.Attach(cellObjectView, true);
        }

        public CellObjectView DeAttach(ICellObject cellObject, CellPosition oldPosition)
        {
            CellView oldCell = _cells[oldPosition.X, oldPosition.Y];
            return oldCell.DeAttach(cellObject);
        }

        public void Destroy(ICellObject cellObject)
        {
            CellView cellView = _cells[cellObject.Owner.Position.X, cellObject.Owner.Position.Y];
            cellView.Destroy(cellObject);
        }
    }
}