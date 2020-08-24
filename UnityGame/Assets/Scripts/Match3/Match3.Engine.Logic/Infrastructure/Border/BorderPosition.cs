using System;

namespace Match3
{
    /// <summary>
    /// Позиция стенки в рамках грида
    /// </summary>
    public readonly struct BorderPosition : IEquatable<BorderPosition>
    {
        public readonly int X;
        public readonly int Y;

        public BorderPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(BorderPosition other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is CellPosition other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public static bool operator ==(BorderPosition left, BorderPosition right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BorderPosition left, BorderPosition right)
        {
            return !left.Equals(right);
        }
    }
}