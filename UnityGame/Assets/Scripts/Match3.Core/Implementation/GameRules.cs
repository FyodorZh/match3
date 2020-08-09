using System.Collections.Generic;
using System.Linq;

namespace Match3.Core
{
    public class GameRules : IGameRules
    {
        private readonly IFeature[] _features;
        
        public IObjectFactory ObjectFactory { get; }
        
        public IViewFactory ViewFactory { get; }
        
        public GameRules(IObjectFactory objectFactory, IViewFactory viewFactory, IEnumerable<IFeature> features)
        {
            ObjectFactory = objectFactory;
            ViewFactory = viewFactory;
            
            _features = features.ToArray();
            foreach (var feature in _features)
            {
                feature.Init(this);
            }
            
            foreach (var feature in _features)
            {
                feature.Start();
            }
        }
    }
}