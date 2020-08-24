namespace Match3
{
    public interface ICellComponent
    {
        string TypeId { get; }

        ICell Cell { get; }

        void Tick(DeltaTime dTime);
    }

    public interface ICellComponentInitializer
    {
        void SetOwner(ICell cell);
    }
}