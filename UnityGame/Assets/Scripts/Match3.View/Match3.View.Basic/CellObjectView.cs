using Match3.Features;
using Match3.ViewBinding.Default;
using UnityEngine;

namespace Match3.View.Default
{
    public class CellObjectView<TObserver> : CellObjectViewBinding<TObserver, IDefaultViewContext>
        where TObserver : class, ICellObjectObserver
    {
        private MoveObjectComponentFeature.IMove _moveComponent;

        protected override void OnInit()
        {
            base.OnInit();
            _moveComponent = Observer.TryGetComponent<MoveObjectComponentFeature.IMove>();
        }

        protected override void Update()
        {
            base.Update();
            if (_moveComponent != null)
            {
                var pos = new Vector3(_moveComponent.Offset.X.ToFloat(), 0, _moveComponent.Offset.Y.ToFloat());
                //Debug.Log("OFFSET " + _cellObject.Owner.Position + "   " + pos);
                transform.localPosition = pos;
            }
        }
    }
}