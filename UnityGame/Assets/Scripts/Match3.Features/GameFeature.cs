using System.Collections.Generic;

namespace Match3.Features
{
    public abstract class GameFeature<TData> : IGameFeature
        where TData : class
    {
        protected IGameRules Rules { get; private set; }

        public string FeatureId { get; }

        protected abstract TData ConstructState(IGame game);

        protected abstract void Process(IGame game, TData state, int dTimeMs);

        protected GameFeature(string featureName)
        {
            FeatureId = featureName;
        }

        public abstract IEnumerable<ICellComponentFeature> DependsOnCellComponentFeatures { get; }
        public abstract IEnumerable<IObjectFeature> DependsOnObjectFeatures { get; }
        public abstract IEnumerable<IObjectComponentFeature> DependsOnObjectComponentFeatures { get; }

        public void Register(IGameRules rules)
        {
            Rules = rules;
        }

        public object InitState(IGame game)
        {
            return ConstructState(game);
        }

        public void Tick(IGame game, object state, int dTimeMs)
        {
            Process(game, (TData)state, dTimeMs);
        }
    }
}