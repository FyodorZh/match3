namespace Match3
{
    public abstract class CellObjectComponent : ICellObjectComponent
    {
        public abstract string TypeId { get; }
        
        public ICellObject Owner { get; private set; }
        
        public void SetOwner(ICellObject owner)
        {
            Owner = owner;
        }
    }
}