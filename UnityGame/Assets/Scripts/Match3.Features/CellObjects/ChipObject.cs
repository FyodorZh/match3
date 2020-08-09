namespace Match3.Features
{
    public class ChipObject : CellObject
    {
        public ChipObject() 
            : base(ObjectTypes.Chip, new Colored(), new Droppable())
        {
        }
    }
}