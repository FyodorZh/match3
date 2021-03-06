﻿using System;

namespace Match3
{
    public readonly struct Time : IEquatable<Time>
    {
        public static readonly Time Zero = new Time();
        public static readonly Time Infinity = new Time(1000_000_000);

        public readonly int Milliseconds;

        public Time(int milliseconds)
        {
            Milliseconds = milliseconds;
        }

        public override string ToString()
        {
            return Milliseconds.ToString();
        }

        public bool Equals(Time other)
        {
            return Milliseconds == other.Milliseconds;
        }

        public override bool Equals(object obj)
        {
            return obj is Time other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Milliseconds;
        }

        public static bool operator ==(Time left, Time right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Time left, Time right)
        {
            return !left.Equals(right);
        }

        public static bool operator <(Time left, Time right)
        {
            return left.Milliseconds < right.Milliseconds;
        }

        public static bool operator <=(Time left, Time right)
        {
            return left.Milliseconds <= right.Milliseconds;
        }

        public static bool operator >(Time left, Time right)
        {
            return left.Milliseconds > right.Milliseconds;
        }

        public static bool operator >=(Time left, Time right)
        {
            return left.Milliseconds >= right.Milliseconds;
        }

        public static DeltaTime operator -(Time left, Time right)
        {
            return new DeltaTime(left.Milliseconds - right.Milliseconds);
        }

        public static Time operator +(Time time, DeltaTime delta)
        {
            return new Time(time.Milliseconds + delta.Milliseconds);
        }

        public static Time operator -(Time time, DeltaTime delta)
        {
            return new Time(time.Milliseconds - delta.Milliseconds);
        }
    }
}