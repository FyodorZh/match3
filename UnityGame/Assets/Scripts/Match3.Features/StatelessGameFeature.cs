namespace Match3.Features
{
    public abstract class StatelessGameFeature : GameFeature<object>
    {
        protected abstract void Process(IGame game, int dTimeMs);
        
        protected StatelessGameFeature(string featureName) 
            : base(featureName)
        {
        }
        
        protected sealed override object ConstructState(IGame game)
        {
            return null;
        }

        protected sealed override void Process(IGame game, object state, int dTimeMs)
        {
            Process(game, dTimeMs);
        }
    }
}