namespace Match3
{
    public interface ICellGrid
    {
        CellGridId Id { get; }
        int Width { get; }
        int Height { get; }
        
        ICell GetCell(CellPosition position);
        IBorder GetBorder(BorderPosition position);
    }
}