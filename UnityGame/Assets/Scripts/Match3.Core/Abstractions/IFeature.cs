namespace Match3
{
    public interface IFeature
    {
        void Init(IGameRules rules);
        void Start(IGame game);
        void Tick(IGame game);
    }
}