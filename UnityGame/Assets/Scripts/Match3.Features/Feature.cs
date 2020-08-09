namespace Match3.Features
{
    public abstract class Feature<TData> : IFeature
        where TData : class
    {
        protected IGameRules Rules { get; private set; }

        protected abstract TData ConstructData();

        protected abstract void Process(IGame game, TData state);
        
        public void Init(IGameRules rules)
        {
            Rules = rules;
        }

        public object Start(IGame game)
        {
            return ConstructData();
        }

        public void Tick(IGame game, object state)
        {
            Process(game, (TData)state);
        }
    }
}