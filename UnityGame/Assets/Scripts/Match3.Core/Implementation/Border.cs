namespace Match3.Core
{
    class Border : IBorder
    {
        private readonly CellGrid _owner;

        public BorderId Id { get; }
        public BorderPosition Position { get; }
        
        public Border(CellGrid owner, BorderPosition position)
        {
            _owner = owner;
            Id = new BorderId(owner.Id, position);
            Position = position;
        }
    }
}