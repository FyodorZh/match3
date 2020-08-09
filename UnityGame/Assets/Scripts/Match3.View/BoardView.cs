using System.Collections.Generic;
using UnityEngine;

namespace Match3.View
{
    public class BoardView : MonoBehaviour
    {
        private IBoard _board;
        
        public GridView _gridViewPrefab;

        private List<GridView> _grids = new List<GridView>();
        public void Setup(IBoard board)
        {
            _board = board;
            foreach (var grid in board.Grids)
            {
                GridView gridView = Instantiate(_gridViewPrefab, transform);
                gridView.name = "grid" + grid.Id.Id;
                gridView.Setup(grid);
                _grids.Add(gridView);
            }
        }
    }
}