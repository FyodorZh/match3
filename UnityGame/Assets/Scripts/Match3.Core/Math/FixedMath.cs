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

        public static Fixed Max(Fixed a, Fixed b)
        {
            return a > b ? a : b;
        }
        
        public static Fixed Min(Fixed a, Fixed b)
        {
            return a < b ? a : b;
        }
    }
}