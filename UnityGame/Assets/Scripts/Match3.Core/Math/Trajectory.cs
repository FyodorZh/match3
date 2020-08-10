namespace Match3.Math
{
    public interface ITrajectory
    {
        FixedVector2 Position { get; set; }

        bool Update(Fixed timeSeconds);
    }
}