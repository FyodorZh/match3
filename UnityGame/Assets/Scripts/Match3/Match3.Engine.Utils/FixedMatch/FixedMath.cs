using System;

public static class FixedMath
{
    public static Fixed Abs(Fixed x)
    {
        if (x > 0)
            return x;
        return -x;
    }

    public static Fixed Max(Fixed a, Fixed b)
    {
        return a > b ? a : b;
    }

    public static Fixed Min(Fixed a, Fixed b)
    {
        return a < b ? a : b;
    }

    public static Fixed Sqrt(Fixed x)
    {
        const long SqrtBase = 1000;
        if (Fixed.Base != SqrtBase * SqrtBase)
            throw new InvalidOperationException();

        long raw = Sqrt(x.GetRaw()) * SqrtBase;
        return Fixed.FromRaw(raw);
    }

    private static long Sqrt(long x)
    {
        if (x < 0)
            throw new InvalidOperationException();
        if (x < 2)
            return x;

        long a = 1;
        long b = 1;

        checked
        {
            while (b * b < x)
            {
                a = b;
                b *= 2;
            }
        }

        while (b - a > 1)
        {
            long m = (a + b) / 2;

            if (m * m <= x)
                a = m;
            else
                b = m;
        }

        if (b * b <= x)
            return b;
        return a;
    }
}