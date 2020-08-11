using System;

namespace Match3
{
    /// <summary>
    /// Позиция ячейки в рамках грида
    /// </summary>
    public readonly struct CellPosition : IEquatable<CellPosition>
    {
        public readonly int X;
        public readonly int Y;

        public CellPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"[{X}, {Y}]";
        }

        public bool Equals(CellPosition other)
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

        public static bool operator ==(CellPosition left, CellPosition right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CellPosition left, CellPosition right)
        {
            return !left.Equals(right);
        }
    }
}