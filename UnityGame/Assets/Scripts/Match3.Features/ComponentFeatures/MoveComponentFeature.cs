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
            FixedVector2 Velocity { get; set; }

            void SetTrajectory(ITrajectory trajectory, Action onUpdate = null, Action onFinish = null);
        }

        public interface IMoveData : ICellObjectComponentData
        {
        }
        
        private class Move : CellObjectComponent, IMove
        {
            private ITrajectory _trajectory;
            private Action _onUpdate;
            private Action _onFinish;

            public override string TypeId => Name;

            public Move(IMoveData data)
            {
            }

            public bool IsMoving => _trajectory != null;
            
            public FixedVector2 Offset { get; private set; }
            public FixedVector2 Velocity { get; set; }
            
            public void SetTrajectory(ITrajectory trajectory, Action onUpdate, Action onFinish)
            {
                Debug.Assert(_trajectory == null);
                Offset = trajectory.Position;
                Velocity = trajectory.Velocity;
                _trajectory = trajectory;
                _onUpdate = onUpdate;
                _onFinish = onFinish;
            }

            protected override void OnRelease()
            {
                if (_trajectory != null)
                {
                    _onFinish?.Invoke(); // ???
                }

                _trajectory = null;
                _onUpdate = null;
                _onFinish = null;
                
                base.OnRelease();
            }

            public override void Tick(Fixed dTimeSeconds)
            {
                if (_trajectory != null)
                {
                    bool inProgress = _trajectory.Update(dTimeSeconds);
                    Offset = _trajectory.Position;
                    Velocity = _trajectory.Velocity;
                    
                    if (_onUpdate != null)
                    {
                        Owner.Owner.Game.InternalInvoke(_onUpdate);
                    }
                    
                    if (!inProgress)
                    {
                        if (_onFinish != null)
                        {
                            var onFinish = _onFinish;
                            Owner.Owner.Game.InternalInvoke(() =>
                            {
                                onFinish();
                                Offset = new FixedVector2(0, 0);
                            });
                        }
                        else
                        {
                            Offset = new FixedVector2(0, 0); 
                        }
                        _trajectory = null;
                        _onUpdate = null;
                        _onFinish = null;
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