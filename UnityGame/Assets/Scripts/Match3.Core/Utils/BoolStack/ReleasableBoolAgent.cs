namespace Match3.Utils
{
    public class ReleasableBoolAgent : IBoolAgent
    {
        public bool Equals(IBoolAgent other)
        {
            return ReferenceEquals(this, other);
        }

        public bool Value { get; } = true;
        public bool IsValid { get; private set; } = true;

        public void Release()
        {
            IsValid = false;
        }
    }
}