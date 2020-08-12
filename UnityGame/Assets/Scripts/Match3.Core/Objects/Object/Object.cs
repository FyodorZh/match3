using Match3.Core;

namespace Match3
{
    public abstract class Object : IObject
    {
        private bool _isReleased;

        private readonly ReleasableLock _lockObject = new ReleasableLock();
        
        public ObjectTypeId TypeId { get; }

        public ILock LockObject => _lockObject;

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
                _lockObject.Release();
                OnRelease();
            }
        }
    }
}