namespace Match3.Core
{
    public abstract class CellComponent : ICellComponent, ICellComponentInitializer
    {
        private ICell _cell;

        public abstract string TypeId { get; }

        public ICell Cell => _cell;

        protected virtual void OnInit()
        {
        }

        public void SetOwner(ICell cell)
        {
            _cell = cell;
            OnInit();
        }

        public virtual void Tick(DeltaTime dTime)
        {
        }
    }
}