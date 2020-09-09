using System;

namespace Match3.Features.Move
{
    public interface IMoveCellObjectComponent : ICellObjectComponent
    {
        bool IsMoving { get; }

        Time LastMoveTime { get; }
        MoveCause MoveCause { get; }

        FixedVector2 Offset { get; set; }
        FixedVector2 Velocity { get; set; }

        void StartMove(MoveCause intention, ITrajectory trajectory, Action onUpdate = null, Action onFinish = null);
    }

    public static class MoveComponentFeature_Ext
    {
        public static FixedVector2 VisualPosition(this IMoveCellObjectComponent move)
        {
            if (move.IsReleased)
                return new FixedVector2();
            var cellPos = move.Owner.Owner.Position;
            return move.Offset + new FixedVector2(cellPos.X, cellPos.Y);
        }
    }

    public abstract class MoveCellObjectComponentFeature : CellObjectComponentFeature
    {
        public const string Name = "Move";

        public sealed override string FeatureId => Name;

        public abstract IMoveCellObjectComponent Construct();
    }

    namespace Default
    {
        public class MoveCellObjectComponentFeatureImpl : MoveCellObjectComponentFeature
        {
            public override IMoveCellObjectComponent Construct()
            {
                return new Move();
            }

            private class Move : CellObjectComponent, IMoveCellObjectComponent
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

                public override ICellObjectComponentData SaveAsData()
                {
                    return VoidCellObjectComponentData.Instance;
                }
            }
        }
    }
}