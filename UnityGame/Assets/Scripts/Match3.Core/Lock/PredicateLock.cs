using System;

namespace Match3.Core
{
    public class PredicateLock : ILock
    {
        private Func<bool> _isActive;
        
        public PredicateLock(Func<bool> isActive)
        {
            _isActive = isActive;
        }
        
        public bool Equals(ILock other)
        {
            return ReferenceEquals(this, other);
        }

        public bool IsActive => _isActive?.Invoke() ?? false;
        public bool IsValid { get; private set; } = true;

        public void Release()
        {
            IsValid = false;
            _isActive = null;
        }
    }
}