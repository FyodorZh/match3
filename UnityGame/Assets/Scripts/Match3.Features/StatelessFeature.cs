namespace Match3.Features
{
    public abstract class StatelessFeature : Feature<object>
    {
        protected abstract void Process(IGame game);
        
        protected sealed override object ConstructData()
        {
            return null;
        }

        protected sealed override void Process(IGame game, object state)
        {
            Process(game);
        }
    }
}