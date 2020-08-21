using System;

namespace Match3
{
    public readonly struct CellOffset : IEquatable<CellOffset>
    {
        public readonly int X;
        public readonly int Y;

        public CellOffset(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"[{X}, {Y}]";
        }

        public bool Equals(CellOffset other)
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

        public static bool operator ==(CellOffset left, CellOffset right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CellOffset left, CellOffset right)
        {
            return !left.Equals(right);
        }
    }
}