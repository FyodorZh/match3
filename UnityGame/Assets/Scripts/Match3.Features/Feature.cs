namespace Match3.Features
{
    public class Feature : IFeature
    {
        protected IGameRules Rules { get; private set; }
        
        public void Init(IGameRules rules)
        {
            Rules = rules;
        }

        public void Start(IGame game)
        {
        }

        public void Tick(IGame game)
        {
        }
    }
}