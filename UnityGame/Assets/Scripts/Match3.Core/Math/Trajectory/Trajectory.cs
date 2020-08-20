namespace Match3
{
    public interface ITrajectory
    {
        FixedVector2 Position { get; }

        FixedVector2 Velocity { get; }

        bool Update(DeltaTime dTime);
    }

    public abstract class Trajectory : ITrajectory
    {
        private bool _finished;

        public FixedVector2 Position { get; protected set; }
        public FixedVector2 Velocity { get; protected set; }

        protected abstract bool OnUpdate(DeltaTime dTime);

        public bool Update(DeltaTime dTime)
        {
            if (_finished)
                return false;

            if (OnUpdate(dTime))
            {
                return true;
            }

            _finished = true;
            return false;
        }
    }
}