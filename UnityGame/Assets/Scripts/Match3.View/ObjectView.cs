using UnityEngine;

namespace Match3.View
{
    public class ObjectView : MonoBehaviour, IObjectView
    {
        public IObject Owner { get; private set; }

        protected virtual void OnInit()
        {
        }

        public void SetOwner(IObject owner)
        {
            Owner = owner;
            OnInit();
        }
        
        public void Release()
        {
            Destroy(this);
        }

        protected virtual void Update()
        {
        }
    }
}