using System;
using Match3.Core;
using Match3;

namespace Match3.Features
{
    public class MoveObjectComponentFeature : ICellObjectObjectComponentFeature
    {
        public const string Name = "Move";
        public static readonly MoveObjectComponentFeature Instance = new MoveObjectComponentFeature();

        public string FeatureId => Name;

        public IMove Construct()
        {
            return new Move();
        }

        public readonly struct MoveCause
        {
            public readonly string Value;

            public MoveCause(string value)
            {
                Value = value;
            }
        }

        public interface IMove : ICellObjectComponent
        {
            bool IsMoving { get; }

            Time LastMoveTime { get; }
            MoveCause MoveCause { get; }

            FixedVector2 Offset { get; set; }
            FixedVector2 Velocity { get; set; }

            void StartMove(MoveCause intention, ITrajectory trajectory, Action onUpdate = null, Action onFinish = null);
        }

        private class Move : CellObjectComponent, IMove
        {
            private ITrajectory _trajectory;
            private Action _onUpdate;
            private Action _onFinish;

            private Time _lastMoveTime;

            public override string TypeId => Name;

            public Move()
            {
            }

            public bool IsMoving => _trajectory != null;

            public Time LastMoveTime => _lastMoveTime;

            public MoveCause MoveCause { get; private set; }

            public FixedVector2 Offset { get; set; }
            public FixedVector2 Velocity { get; set; }

            public void StartMove(MoveCause intention, ITrajectory trajectory, Action onUpdate, Action onFinish)
            {
                Debug.Assert(_trajectory == null);
                Offset = trajectory.Position;
                Velocity = trajectory.Velocity;
                _trajectory = trajectory;
                _onUpdate = onUpdate;
                _onFinish = onFinish;
                MoveCause = intention;
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

            public override void Tick(DeltaTime dTime)
            {
                if (_trajectory != null)
                {
                    bool inProgress = _trajectory.Update(dTime);
                    Offset = _trajectory.Position;
                    Velocity = _trajectory.Velocity;

                    _lastMoveTime = Owner.Owner.Game.CurrentTime;

                    if (_onUpdate != null)
                    {
                        Owner.Owner.Game.InternalInvoke(_onUpdate);
                    }

                    if (!inProgress)
                    {
                        if (_onFinish != null)
                        {
                            Owner.Owner.Game.InternalInvoke(_onFinish);
                        }
                        _trajectory = null;
                        _onUpdate = null;
                        _onFinish = null;
                    }
                }
                else
                {
                    Velocity = new FixedVector2(0, 0);
                    //Offset = new FixedVector2(0, 0);
                }
            }
        }
    }

    public static class MoveComponentFeature_Ext
    {
        public static FixedVector2 VisualPosition(this MoveObjectComponentFeature.IMove move)
        {
            if (move.IsReleased)
                return new FixedVector2();
            var cellPos = move.Owner.Owner.Position;
            return move.Offset + new FixedVector2(cellPos.X, cellPos.Y);
        }
    }
}