using System;

namespace Match3.Math
{
    public readonly struct Fixed : IEquatable<Fixed>
    {
        public const long Base = 1000_000;
        private const double InvBase = 1.0 / Base;

        private readonly long _value;

        private Fixed(long value)
        {
            _value = value;
        }

        public Fixed(int nominator, int denominator)
        {
            _value = (nominator * Base) / denominator;
        }

        public override string ToString()
        {
            return ToFloat().ToString();
        }

        public float ToFloat()
        {
            return (float)(_value * InvBase);
        }

        public long GetRaw()
        {
            return _value;
        }

        public static Fixed FromRaw(long raw)
        {
            return new Fixed(raw);
        }
        
        public static Fixed operator /(Fixed left, Fixed right)
        {
            return new Fixed(left._value * Base / right._value);
        }

        public bool Equals(Fixed other)
        {
            return _value == other._value;
        }

        public override bool Equals(object obj)
        {
            return obj is Fixed other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static bool operator ==(Fixed left, Fixed right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Fixed left, Fixed right)
        {
            return !left.Equals(right);
        }
        
        public static bool operator <(Fixed left, Fixed right)
        {
            return left._value < right._value;
        }
        
        public static bool operator >(Fixed left, Fixed right)
        {
            return left._value > right._value;
        }
        
        public static bool operator <=(Fixed left, Fixed right)
        {
            return left._value <= right._value;
        }
        
        public static bool operator >=(Fixed left, Fixed right)
        {
            return left._value >= right._value;
        }

        public static implicit operator Fixed(int value)
        {
            return new Fixed(value * Base);
        }

        public static Fixed operator +(Fixed number)
        {
            return number;
        }
        
        public static Fixed operator -(Fixed number)
        {
            return new Fixed(-number._value);
        }

        public static Fixed operator +(Fixed left, Fixed right)
        {
            return new Fixed(left._value + right._value);
        }
        
        public static Fixed operator -(Fixed left, Fixed right)
        {
            return new Fixed(left._value - right._value);
        }
        
        public static Fixed operator *(Fixed left, Fixed right)
        {
            return new Fixed(left._value * right._value / Base);
        }
    }
}