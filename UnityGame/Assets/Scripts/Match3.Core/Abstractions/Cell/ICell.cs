using System;
using System.Collections.Generic;

namespace Match3
{
    public interface ICell
    {
        event Action<ICellObject> ContentAdded;
        
        CellId Id { get; }
        CellPosition Position { get; }
        
        IGame Game { get; }
        IGrid Owner { get; }
        
        bool IsActive { get; set; }
        
        IReadOnlyList<ICellObject> Content { get; }
        //ICellObject TryGetContent(string typeId);

        bool TryAddContent(ICellObject cellObject);
    }
}