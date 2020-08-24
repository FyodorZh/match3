using UnityEngine;

namespace Match3.View
{
    public class ObjectView : MonoBehaviour, IObjectView
    {
        public IObjectObserver Owner { get; private set; }

        protected virtual void OnInit()
        {
        }

        public void SetOwner(IObjectObserver owner)
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