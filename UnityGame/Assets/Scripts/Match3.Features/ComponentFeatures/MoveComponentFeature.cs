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

            void Update(Fixed dTimeSeconds);
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
                _trajectory = trajectory;
            }

            public void Update(Fixed dTimeSeconds)
            {
                if (_trajectory != null)
                {
                    _trajectory.Update(dTimeSeconds);
                    Velocity = _trajectory.Position - Offset;
                    Offset = _trajectory.Position;
                }
            }
        }
    }
}