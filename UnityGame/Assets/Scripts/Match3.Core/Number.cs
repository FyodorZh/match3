namespace Match3.Core
{
    public readonly struct Number
    {
        private const long Base = 1000;
        private const double InvBase = 1.0 / Base;

        private readonly long _value;

        private Number(long value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return ToFloat().ToString();
        }

        public float ToFloat()
        {
            return (float)(_value * InvBase);
        }

        public static implicit operator Number(int value)
        {
            return new Number(value * Base);
        }

        public static Number operator +(Number number)
        {
            return number;
        }
        
        public static Number operator -(Number number)
        {
            return new Number(-number._value);
        }

        public static Number operator +(Number left, Number right)
        {
            return new Number(left._value + right._value);
        }
        
        public static Number operator -(Number left, Number right)
        {
            return new Number(left._value - right._value);
        }
        
        public static Number operator *(Number left, Number right)
        {
            return new Number(left._value * right._value / Base);
        }
        
        public static Number operator /(Number left, Number right)
        {
            return new Number(left._value * Base / right._value);
        }
    }
}