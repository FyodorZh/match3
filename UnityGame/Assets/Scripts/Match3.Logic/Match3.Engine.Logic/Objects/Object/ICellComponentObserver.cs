namespace Match3
{
    public interface ICellComponentObserver : IObserver
    {
        ICellObserver Owner { get; }
    }
}