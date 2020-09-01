namespace Match3
{
    public interface IBorder : IBorderObserver
    {
        IBorderObject Content { get; }
        void SetContent(IBorderObject newObject);
    }
}