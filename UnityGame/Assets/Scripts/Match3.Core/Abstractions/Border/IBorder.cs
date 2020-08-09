namespace Match3
{
    public interface IBorder
    {
        BorderId Id { get; }
        BorderPosition Position { get; }

        IBorderObject Content { get; }
        void SetContent(IBorderObject newObject);
    }
}