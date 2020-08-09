namespace Match3
{
    public interface IFeature
    {
        void Init(IGameRules rules);
        object Start(IGame game);
        void Tick(IGame game, object state, int dTimeMs);
    }
}