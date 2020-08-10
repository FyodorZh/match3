namespace Match3.Math
{
    public interface ITrajectory
    {
        FixedVector2 Offset { get; set; }
        FixedVector2 Velocity { get; set; }

        bool Update(Fixed timeSeconds);
    }
}