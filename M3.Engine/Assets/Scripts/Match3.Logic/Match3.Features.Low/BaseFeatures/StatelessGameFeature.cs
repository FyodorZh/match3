namespace Match3.Features
{
    public abstract class StatelessGameFeature : GameFeature<object>
    {
        protected abstract void Process(IGame game, DeltaTime dTime);

        protected StatelessGameFeature(string featureName)
            : base(featureName)
        {
        }

        protected sealed override object ConstructState(IGame game)
        {
            return null;
        }

        protected sealed override void Process(IGame game, object state, DeltaTime dTime)
        {
            Process(game, dTime);
        }
    }
}