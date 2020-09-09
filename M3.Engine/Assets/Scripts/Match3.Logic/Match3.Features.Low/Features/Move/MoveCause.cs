using System;

namespace Match3.Features.Move
{
    public readonly struct MoveCause : IEquatable<MoveCause>
    {
        public readonly string Value;

        public MoveCause(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }

        public bool Equals(MoveCause other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is MoveCause other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }

        public static bool operator ==(MoveCause left, MoveCause right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MoveCause left, MoveCause right)
        {
            return !left.Equals(right);
        }
    }
}