using System;

namespace Match3.Utils
{
    public interface IBoolAgent : IEquatable<IBoolAgent>
    {
        bool Value { get; }
        bool IsValid { get; }
    }
}