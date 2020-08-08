namespace Match3
{
    public interface ICellGrid
    {
        CellGridId Id { get; }
        int Width { get; }
        int Height { get; }
        
        ICell At(CellPosition position);
    }
}