using System;

namespace Match3
{
    public readonly struct CellId : IEquatable<CellId>
    {
        public readonly CellGridId CellGridId;
        public readonly CellPosition Position;

        public CellId(CellGridId cellGridId, CellPosition position)
        {
            CellGridId = cellGridId;
            Position = position;
        }

        public bool Equals(CellId other)
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