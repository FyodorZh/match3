namespace Match3.Math
{
    public interface ITrajectory
    {
        FixedVector2 Position { get; }
        
        FixedVector2 Velocity { get; }

        bool Update(Fixed timeSeconds);

        void Finish();
    }
}