using System;

namespace Match3
{
    public readonly struct CellId : IEquatable<CellId>
    {
        public readonly GridId GridId;
        public readonly CellPosition Position;

        public CellId(GridId gridId, CellPosition position)
        {
            GridId = gridId;
            Position = position;
        }

        public bool Equals(CellId other)
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

        public static bool operator ==(CellId left, CellId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CellId left, CellId right)
        {
            return !left.Equals(right);
        }
    }
}