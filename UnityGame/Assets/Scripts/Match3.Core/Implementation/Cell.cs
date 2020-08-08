namespace Match3.Core
{
    class Cell : ICell
    {
        private readonly CellGrid _owner;
        
        public CellId Id { get; }
        
        public CellPosition Position { get; }
        
        public ICellGrid Owner => _owner;

        public Cell(CellGrid owner, CellPosition position)
        {
            _owner = owner;
            Id = new CellId(owner.Id, position);
            Position = position;
        }
    }
}