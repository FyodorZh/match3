using System.Collections.Generic;

namespace Match3.Core
{
    public class LockStack
    {
        private readonly List<ILock> _lockObjects = new List<ILock>();
        
        public bool IsLocked
        {
            get
            {
                int count = _lockObjects.Count;
                if (count > 0)
                {
                    for (int i = count - 1; i >= 0; --i)
                    {
                        var lob = _lockObjects[i];
                        if (!lob.IsValid)
                        {
                            _lockObjects[i] = _lockObjects[count - 1];
                            _lockObjects.RemoveAt(count - 1);
                            count -= 1;
                        }
                        else
                        {
                            if (lob.IsActive)
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }
        }
        
        public void AddLock(ILock lockObject)
        {
            Debug.Assert(!_lockObjects.Contains(lockObject));
            _lockObjects.Add(lockObject);
        }

        public void RemoveLock(ILock lockObject)
        {
            _lockObjects.Remove(lockObject);
        }
    }
}