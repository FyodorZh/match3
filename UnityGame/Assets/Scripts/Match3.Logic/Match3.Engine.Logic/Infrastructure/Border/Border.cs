namespace Match3.Logic
{
    class Border : IBorder
    {
        private readonly Board _owner;

        public BorderPosition Position { get; }

        public IBorderObject Content { get; private set; }

        public Border(Board owner, BorderPosition position)
        {
            _owner = owner;
            Position = position;
        }

        public void SetContent(IBorderObject newObject)
        {
            Content = newObject;
        }
    }
}