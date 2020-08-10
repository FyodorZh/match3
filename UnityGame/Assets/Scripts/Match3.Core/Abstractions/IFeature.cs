namespace Match3
{
    public interface IFeature
    {
        void Register(IGameRules rules);
        
        object InitState(IGame game);
        void Tick(IGame game, object state, int dTimeMs);
    }
}