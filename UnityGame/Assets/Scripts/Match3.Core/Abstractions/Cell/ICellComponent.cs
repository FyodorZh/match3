using Match3;

namespace Match3.Core
{
    public interface ICellComponent
    {
        string TypeId { get; }

        ICell Cell { get; }

        void Tick(Fixed dTimeSeconds);
    }

    public interface ICellComponentInitializer
    {
        void SetOwner(ICell cell);
    }
}