using Match3.ViewBinding.Default;
using UnityEngine;

namespace Match3.View.Default
{
    public class CellView : CellViewBinding
    {
        public GameObject _cellViewActive;
        public GameObject _cellViewInactive;

        public GameObject _cellViewLock;

        public CellTouch _cellTouch;

        protected override void OnInit()
        {
            var cellPosition = Cell.Position;
            name = "cell." + cellPosition.X + "x" + cellPosition.Y;

            _cellTouch.Setup(Cell, GameController);

            _cellViewActive.SetActive(Cell.IsActive);
            _cellViewInactive.SetActive(!Cell.IsActive);
        }

        protected override void Update()
        {
            base.Update();
            _cellViewLock.SetActive(Cell.IsLocked);
        }
    }
}