namespace Match3
{
    public interface ICell
    {
        CellId Id { get; }
        CellPosition Position { get; }
        
        IGrid Owner { get; }
    }
}