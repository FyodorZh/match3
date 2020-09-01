using UnityEngine;

namespace Match3.ViewBinding.Default
{
    public class ObjectViewBinding : MonoBehaviour
    {
        public IObjectObserver Owner { get; private set; }

        public void Init(IObjectObserver owner)
        {
            Owner = owner;
            OnInit();
        }

        protected virtual void OnInit()
        {
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