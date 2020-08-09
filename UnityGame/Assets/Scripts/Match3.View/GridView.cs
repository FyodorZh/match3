using UnityEngine;

namespace Match3.View
{
    public class GridView : MonoBehaviour
    {
        private IGrid _grid;

        public CellView _cellViewPrefab;
        
        public void Setup(IGrid grid)
        {
            _grid = grid;

            foreach (var cell in grid.AllCells)
            {
                if (cell.IsActive)
                {
                    CellView view = Instantiate(_cellViewPrefab, transform);
                    view.name = "cell." + cell.Position.X + "x" + cell.Position.Y;
                    view.transform.position = new Vector3(cell.Position.X, 0, cell.Position.Y);
                    view.Setup(cell);
                }
            }
        }
    }
}