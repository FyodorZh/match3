using System;

namespace Match3.Math
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
        
        protected override bool OnUpdate(Fixed timeSeconds)
        {
            bool inProgress = true;
            
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