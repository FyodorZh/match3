namespace Match3
{
    public interface ICellObjectComponentData : IObjectComponentData
    {
    }

    public class VoidCellObjectComponentData : ICellObjectComponentData
    {
        public static readonly VoidCellObjectComponentData Instance = new VoidCellObjectComponentData();
    }
}