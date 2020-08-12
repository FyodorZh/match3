using System;

namespace Match3.Utils
{
    public class PredicateBoolAgent : IBoolAgent
    {
        private Func<bool> _isActive;
        
        public PredicateBoolAgent(Func<bool> isActive)
        {
            _isActive = isActive;
        }
        
        public bool Equals(IBoolAgent other)
        {
            return ReferenceEquals(this, other);
        }

        public bool Value => _isActive?.Invoke() ?? false;
        public bool IsValid { get; private set; } = true;

        public void Release()
        {
            IsValid = false;
            _isActive = null;
        }
    }
}