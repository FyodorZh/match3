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
            var trajectory = _moveComponent.Trajectory;
            if (trajectory != null)
            {
                transform.localPosition = new Vector3(trajectory.Offset.X.ToFloat(), 0, trajectory.Offset.Y.ToFloat());
            }
            else
            {
                transform.localPosition = Vector3.zero;
            }
        }
    }
}