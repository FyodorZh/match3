using System.Collections.Generic;

namespace Match3
{
    public interface ICell
    {
        CellId Id { get; }
        CellPosition Position { get; }
        
        IGrid Owner { get; }
        
        bool IsActive { get; set; }
        
        IReadOnlyList<ICellObject> Content { get; }

        bool TryAddContent(ICellObject cellObject);
    }
}