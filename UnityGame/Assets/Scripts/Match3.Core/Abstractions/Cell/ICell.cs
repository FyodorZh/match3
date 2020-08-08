namespace Match3
{
    public interface ICell
    {
        CellId Id { get; }
        CellPosition Position { get; }
        
        ICellGrid Owner { get; }
    }
}