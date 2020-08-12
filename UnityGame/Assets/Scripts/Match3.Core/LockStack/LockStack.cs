using System.Collections.Generic;
using Match3.Utils;

namespace Match3.Core
{
    public class LockStack
    {
        private readonly BoolStack _boolStack = new BoolStack();

        public bool IsLocked => _boolStack.Value;
        
        public void AddLock(ILock lockObject)
        {
            _boolStack.AddAgent(lockObject);
        }

        public void RemoveLock(ILock lockObject)
        {
            _boolStack.RemoveAgent(lockObject);
        }
    }
}