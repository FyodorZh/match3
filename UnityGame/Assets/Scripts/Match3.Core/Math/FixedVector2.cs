namespace Match3.Math
{
    public struct FixedVector2
    {
        public Fixed X;
        public Fixed Y;

        public FixedVector2(Fixed x, Fixed y)
        {
            X = x;
            Y = y;
        }

        public static FixedVector2 operator +(FixedVector2 left, FixedVector2 right)
        {
            return new FixedVector2(left.X + right.X, left.Y + right.Y);
        }
        
        public static FixedVector2 operator -(FixedVector2 left, FixedVector2 right)
        {
            return new FixedVector2(left.X - right.X, left.Y - right.Y);
        }
    }
}