namespace Match3.Core
{
    public abstract class Object : IObject
    {
        private bool _isReleased;

        public ObjectTypeId TypeId { get; }

        protected abstract void OnRelease();

        protected Object(ObjectTypeId type)
        {
            TypeId = type;
        }

        public void Release()
        {
            if (!_isReleased)
            {
                _isReleased = true;
                OnRelease();
            }
        }
    }
}