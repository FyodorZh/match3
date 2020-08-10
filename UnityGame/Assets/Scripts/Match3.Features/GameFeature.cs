namespace Match3.Features
{
    public abstract class GameFeature<TData> : IGameFeature
        where TData : class
    {
        protected IGameRules Rules { get; private set; }

        public string Name { get; }
        
        protected abstract TData ConstructData();

        protected abstract void Process(IGame game, TData state, int dTimeMs);

        protected GameFeature(string featureName)
        {
            Name = featureName;
        }
        
        public void Register(IGameRules rules)
        {
            Rules = rules;
        }

        public object InitState(IGame game)
        {
            return ConstructData();
        }

        public void Tick(IGame game, object state, int dTimeMs)
        {
            Process(game, (TData)state, dTimeMs);
        }
    }
}