namespace Match3
{
    public abstract class CellObjectComponent : ICellObjectComponent
    {
        public abstract string TypeId { get; }
        public bool IsReleased { get; private set; }

        public ICellObject Owner { get; private set; }
        ICellObjectObserver ICellObjectComponentObserver.Owner => Owner;

        public void SetOwner(ICellObject owner)
        {
            Owner = owner;
        }

        public void Release()
        {
            if (!IsReleased)
            {
                IsReleased = true;
                Owner = null;
                OnRelease();
            }
        }

        protected virtual void OnRelease()
        {
            // DO NOTHING
        }

        public virtual void Tick(DeltaTime dTime)
        {
            // DO NOTHING
        }

        public abstract ICellObjectComponentData SaveAsData();
    }
}