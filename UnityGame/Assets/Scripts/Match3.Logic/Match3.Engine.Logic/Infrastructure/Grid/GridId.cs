using System;

namespace Match3
{
    /// <summary>
    /// Уникальный идентификатор грида в рамках доски
    /// </summary>
    public readonly struct GridId : IEquatable<GridId>
    {
        public static readonly GridId Invalid = new GridId();
        
        public readonly int Id;

        public GridId(int id)
        {
            Id = id;
        }

        public bool Equals(GridId other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is GridId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(GridId left, GridId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GridId left, GridId right)
        {
            return !left.Equals(right);
        }
    }
}