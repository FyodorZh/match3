using System;

namespace Match3
{
    public class LineTrajectory : Trajectory
    {
        public static readonly Func<Fixed, Fixed> Linear = time => time;

        private readonly FixedVector2 _from;
        private readonly FixedVector2 _delta;
        private readonly Fixed _timeBudget;
        private readonly Func<Fixed, Fixed> _timeFunction;

        private Fixed _time;

        public LineTrajectory(FixedVector2 from, FixedVector2 to, Fixed timeBudget, Func<Fixed, Fixed> timeFunction)
        {
            _from = from;
            _delta = to - from;
            _timeBudget = timeBudget;
            _timeFunction = timeFunction;

            Fixed t = timeFunction(0);

            Position = _from + _delta * t;
            Velocity = new FixedVector2(0, 0);
        }

        protected override bool OnUpdate(DeltaTime dTime)
        {
            bool inProgress = true;

            var timeSeconds = new Fixed(dTime.Milliseconds, 1000);

            _time += timeSeconds;
            if (_time >= _timeBudget)
            {
                _time = _timeBudget;
                inProgress = false;
            }

            var t = _timeFunction(_time / _timeBudget);

            var oldPos = Position;
            Position = _from + _delta * t;
            Velocity = (Position - oldPos) / timeSeconds;

            return inProgress;
        }
    }
}