namespace Match3
{
    public interface ITrajectory
    {
        FixedVector2 Position { get; }
        
        FixedVector2 Velocity { get; }

        bool Update(Fixed timeSeconds);
    }
    
    public abstract class Trajectory : ITrajectory
    {
        private bool _finished;
            
        public FixedVector2 Position { get; protected set; }
        public FixedVector2 Velocity { get; protected set; }

        protected abstract bool OnUpdate(Fixed timeSeconds);
            
        public bool Update(Fixed timeSeconds)
        {
            if (_finished)
                return false;
            
            if (OnUpdate(timeSeconds))
            {
                return true;
            }

            _finished = true;
            return false;
        }
    }
}