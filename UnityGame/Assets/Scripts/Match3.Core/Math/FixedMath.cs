namespace Match3.Math
{
    public static class FixedMath
    {
        public static Fixed Abs(Fixed x)
        {
            if (x > 0)
                return x;
            return -x;
        }
    }
}