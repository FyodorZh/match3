using System;
namespace Match3.Utils
{
    public class PredicateLock : PredicateBoolAgent, ILock
    {
        public PredicateLock(Func<bool> isActive)
            : base(isActive)
        {
        }
    }
}