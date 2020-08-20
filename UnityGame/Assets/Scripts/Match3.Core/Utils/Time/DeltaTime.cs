using System;

namespace Match3
{
    public readonly struct DeltaTime : IEquatable<DeltaTime>
    {
        public static readonly DeltaTime Zero = new DeltaTime();

        public readonly int Milliseconds;

        public DeltaTime(int milliseconds)
        {
            Milliseconds = milliseconds;
        }

        public bool Equals(DeltaTime other)
        {
            return Milliseconds == other.Milliseconds;
        }

        public override bool Equals(object obj)
        {
            return obj is DeltaTime other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Milliseconds;
        }

        public static bool operator ==(DeltaTime left, DeltaTime right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DeltaTime left, DeltaTime right)
        {
            return !left.Equals(right);
        }

        public static bool operator <(DeltaTime left, DeltaTime right)
        {
            return left.Milliseconds < right.Milliseconds;
        }

        public static bool operator <=(DeltaTime left, DeltaTime right)
        {
            return left.Milliseconds <= right.Milliseconds;
        }

        public static bool operator >(DeltaTime left, DeltaTime right)
        {
            return left.Milliseconds > right.Milliseconds;
        }

        public static bool operator >=(DeltaTime left, DeltaTime right)
        {
            return left.Milliseconds >= right.Milliseconds;
        }

        public static DeltaTime operator +(DeltaTime left, DeltaTime right)
        {
            return new DeltaTime(left.Milliseconds + right.Milliseconds);
        }

        public static DeltaTime operator -(DeltaTime left, DeltaTime right)
        {
            return new DeltaTime(left.Milliseconds - right.Milliseconds);
        }
    }
}