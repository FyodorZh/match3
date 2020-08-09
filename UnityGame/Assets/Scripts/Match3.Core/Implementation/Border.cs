namespace Match3.Core
{
    class Border : IBorder
    {
        private readonly Grid _owner;

        public BorderId Id { get; }
        public BorderPosition Position { get; }
        
        public Border(Grid owner, BorderPosition position)
        {
            _owner = owner;
            Id = new BorderId(owner.Id, position);
            Position = position;
        }
    }
}