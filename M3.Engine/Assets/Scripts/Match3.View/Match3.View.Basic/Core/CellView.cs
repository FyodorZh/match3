using Match3.ViewBinding.Default;
using UnityEngine;

namespace Match3.View.Default
{
    public class CellView : CellViewBinding<IDefaultViewContext>
    {
        public GameObject _cellViewActive;
        public GameObject _cellViewInactive;

        public GameObject _cellViewLock;

        public CellTouch _cellTouch;

        protected override void OnInit()
        {
            base.OnInit();
            
            var cellPosition = Observer.Position;
            name = "cell." + cellPosition.X + "x" + cellPosition.Y;

            _cellTouch.Setup(Observer, ViewContext.GameController);

            _cellViewActive.SetActive(Observer.IsActive);
            _cellViewInactive.SetActive(!Observer.IsActive);
        }

        protected override void Update()
        {
            base.Update();
            _cellViewLock.SetActive(Observer.IsLocked);
        }
    }
}