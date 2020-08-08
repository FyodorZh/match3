using System.Collections.Generic;
using System.Linq;

namespace Match3.Core
{
    public class GameRules : IGameRules
    {
        private readonly IFeature[] _features;
        
        public GameRules(IEnumerable<IFeature> features)
        {
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