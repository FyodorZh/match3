namespace Match3.Core
{
    class Cell : ICell
    {
        private readonly Grid _owner;
        
        public CellId Id { get; }
        
        public CellPosition Position { get; }
        
        public IGrid Owner => _owner;

        public Cell(Grid owner, CellPosition position)
        {
            _owner = owner;
            Id = new CellId(owner.Id, position);
            Position = position;
        }
    }
}