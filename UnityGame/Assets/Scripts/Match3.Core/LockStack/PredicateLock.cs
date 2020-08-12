using System;
using Match3.Utils;

namespace Match3.Core
{
    public class PredicateLock : PredicateBoolAgent, ILock
    {
        public PredicateLock(Func<bool> isActive)
            : base(isActive)
        {
        }
    }
}