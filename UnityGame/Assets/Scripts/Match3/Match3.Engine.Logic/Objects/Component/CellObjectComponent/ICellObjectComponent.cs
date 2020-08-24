namespace Match3
{
    public interface ICellObjectComponent : IObjectComponent, ICellObjectComponentObserver
    {
        new ICellObject Owner { get; }
        void SetOwner(ICellObject owner);

        void Tick(DeltaTime dTime);
    }
}