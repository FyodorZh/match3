using Match3.Features;
using UnityEngine;

namespace Match3.View
{
    public class CellObjectView : ObjectView, ICellObjectView
    {
        private ICellObject _cellObject;
        private MoveComponentFeature.IMove _moveComponent;
        
        protected override void OnInit()
        {
            base.OnInit();
            _cellObject = (ICellObject)Owner;
            _moveComponent = _cellObject.TryGetComponent<MoveComponentFeature.IMove>();
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