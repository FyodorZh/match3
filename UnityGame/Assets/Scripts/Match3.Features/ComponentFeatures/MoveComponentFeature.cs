using System;
using Match3.Core;
using Match3.Math;

namespace Match3.Features
{
    public class MoveComponentFeature : ICellObjectComponentFeature
    {
        public const string Name = "Move"; 
        public static readonly MoveComponentFeature Instance = new MoveComponentFeature();

        public string FeatureId => Name;
        
        public IObjectComponent Construct(IObjectComponentData data)
        {
            if (!(data is IMoveData MoveData))
                throw new InvalidOperationException();
            
            return Construct(MoveData);
        }

        public IMove Construct(IMoveData data)
        {
            return new Move(data);
        }
        
        public interface IMove : ICellObjectComponent
        {
            bool IsMoving { get; }
            
            FixedVector2 Offset { get; }
            FixedVector2 Velocity { get; }

            void SetTrajectory(ITrajectory trajectory);
        }

        public interface IMoveData : ICellObjectComponentData
        {
        }
        
        private class Move : CellObjectComponent, IMove
        {
            private ITrajectory _trajectory;
            
            public override string TypeId => Name;

            public Move(IMoveData data)
            {
            }

            public bool IsMoving => _trajectory != null;
            
            public FixedVector2 Offset { get; private set; }
            public FixedVector2 Velocity { get; private set; }
            public void SetTrajectory(ITrajectory trajectory)
            {
                Debug.Assert(_trajectory == null);
                Offset = trajectory.Position;
                Velocity = trajectory.Velocity;
                _trajectory = trajectory;
            }

            protected override void OnRelease()
            {
                _trajectory?.Finish();
                base.OnRelease();
            }

            public override void Tick(Fixed dTimeSeconds)
            {
                if (_trajectory != null)
                {
                    bool inProgress = _trajectory.Update(dTimeSeconds);
                    Offset = _trajectory.Position;
                    Velocity = _trajectory.Velocity;
                    if (!inProgress)
                    {
                        _trajectory = null;
                    }
                }
                else
                {
                    Velocity = new FixedVector2(0, 0);
                }
            }
        }
    }
}