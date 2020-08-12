namespace Match3.Core
{
    public class ReleasableLock : ILock
    {
        public bool Equals(ILock other)
        {
            return ReferenceEquals(this, other);
        }

        public bool IsActive => true;
        public bool IsValid { get; private set; } = true;

        public void Release()
        {
            IsValid = false;
        }
    }
}