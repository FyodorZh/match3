using System;

namespace Match3.Core
{
    public interface ILock : IEquatable<ILock>
    {
        bool IsActive { get; }
        bool IsValid { get; }
    }
}