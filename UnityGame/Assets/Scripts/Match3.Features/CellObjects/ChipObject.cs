using System.Collections.Generic;

namespace Match3.Features.CellObjects
{
    public class ChipObject : CellObject
    {
        public ChipObject() 
            : base(ObjectTypes.Chip, new Colored(), new Droppable())
        {
        }
    }
}