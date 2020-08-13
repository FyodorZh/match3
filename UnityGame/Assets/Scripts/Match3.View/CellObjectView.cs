using Match3.Features;
using UnityEngine;

namespace Match3.View
{
    public class CellObjectView : ObjectView, ICellObjectView
    {
        private ICellObject _cellObject;
        private MoveObjectComponentFeature.IMove _moveComponent;

        protected ICellObject CellObject => _cellObject;
        
        protected override void OnInit()
        {
            base.OnInit();
            _cellObject = (ICellObject)Owner;
            _moveComponent = _cellObject.TryGetComponent<MoveObjectComponentFeature.IMove>();
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