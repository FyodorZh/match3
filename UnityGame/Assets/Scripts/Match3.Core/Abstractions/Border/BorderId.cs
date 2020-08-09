using System;

namespace Match3
{
    public readonly struct BorderId : IEquatable<BorderId>
    {
        public readonly CellGridId CellGridId;
        public readonly BorderPosition Position;

        public BorderId(CellGridId cellGridId, BorderPosition position)
        {
            CellGridId = cellGridId;
            Position = position;
        }

        public bool Equals(BorderId other)
        {
            return CellGridId.Equals(other.CellGridId) && Position.Equals(other.Position);
        }

        public override bool Equals(object obj)
        {
            return obj is CellId other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (CellGridId.GetHashCode() * 397) ^ Position.GetHashCode();
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