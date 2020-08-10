using UnityEngine;

namespace Match3.View
{
    public class ObjectView : MonoBehaviour, IObjectView
    {
        public IObject Owner { get; private set; }

        public void SetOwner(IObject owner)
        {
            Owner = owner;
        }
        
        public void Release()
        {
            Destroy(this);
        }
    }
}