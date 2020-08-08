using System;

namespace Match3
{
    /// <summary>
    /// Уникальный идентификатор грида в рамках доски
    /// </summary>
    public readonly struct CellGridId : IEquatable<CellGridId>
    {
        public static readonly CellGridId Invalid = new CellGridId();
        
        public readonly int Id;

        public CellGridId(int id)
        {
            Id = id;
        }

        public bool Equals(CellGridId other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is CellGridId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(CellGridId left, CellGridId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CellGridId left, CellGridId right)
        {
            return !left.Equals(right);
        }
    }
}