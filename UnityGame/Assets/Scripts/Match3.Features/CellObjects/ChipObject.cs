using System.Collections.Generic;

namespace Match3.Features.CellObjects
{
    public class ChipObject : CellObject
    {
        public ChipObject(ObjectTypeId type, IEnumerable<ICellObjectFeature> features) 
            : base(type, new Colored(), new Droppable())
        {
        }
    }
}