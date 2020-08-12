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

        public FixedVector2 Normalized
        {
            get
            {
                Fixed len = FixedMath.Sqrt(X * X + Y * Y);
                return new FixedVector2(X / len, Y / len);
            }
        }
        
        public static FixedVector2 operator +(FixedVector2 v)
        {
            return v;
        }
        
        public static FixedVector2 operator -(FixedVector2 v)
        {
            return new FixedVector2(-v.X, -v.Y);
        }

        public static FixedVector2 operator +(FixedVector2 left, FixedVector2 right)
        {
            return new FixedVector2(left.X + right.X, left.Y + right.Y);
        }
        
        public static FixedVector2 operator -(FixedVector2 left, FixedVector2 right)
        {
            return new FixedVector2(left.X - right.X, left.Y - right.Y);
        }
        
        public static FixedVector2 operator *(FixedVector2 left, Fixed right)
        {
            return new FixedVector2(left.X * right, left.Y * right);
        }
        
        public static FixedVector2 operator *(Fixed left, FixedVector2 right)
        {
            return new FixedVector2(right.X * left, right.Y * left);
        }
        
        public static FixedVector2 operator /(FixedVector2 left, Fixed right)
        {
            return new FixedVector2(left.X / right, left.Y / right);
        }
    }
}