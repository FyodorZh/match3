using System.Collections.Generic;

namespace Match3.Core
{
    public class GameRules : IGameRules
    {
        private readonly List<IGameFeature> _gameFeatures;
        private readonly List<IObjectFeature> _objectFeatures;
        
        public IObjectFactory ObjectFactory { get; }
        
        public IViewFactory ViewFactory { get; }
        
        public IReadOnlyList<IGameFeature> GameFeatures => _gameFeatures;

        public GameRules(IObjectFactory objectFactory, IViewFactory viewFactory)
        {
            ObjectFactory = objectFactory;
            ViewFactory = viewFactory;
        }
        
        public void RegisterGameFeature(IGameFeature feature)
        {
            _gameFeatures.Add(feature);
            feature.Register(this);
        }

        public void RegisterObjectFeature(IObjectFeature feature)
        {
            _objectFeatures.Add(feature);
        }

        public void RegisterComponentFeature(IComponentFeature feature)
        {
            
        }

        public void BakeAllFeatures()
        {
            // TODO
        }
    }
}