using System;

namespace Match3
{
    public readonly struct BorderId : IEquatable<BorderId>
    {
        public readonly GridId GridId;
        public readonly BorderPosition Position;

        public BorderId(GridId gridId, BorderPosition position)
        {
            GridId = gridId;
            Position = position;
        }

        public bool Equals(BorderId other)
        {
            return GridId.Equals(other.GridId) && Position.Equals(other.Position);
        }

        public override bool Equals(object obj)
        {
            return obj is CellId other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (GridId.GetHashCode() * 397) ^ Position.GetHashCode();
            }
        }

        public static bool operator ==(BorderId left, BorderId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BorderId left, BorderId right)
        {
            return !left.Equals(right);
        }
    }
}