using System.Collections.Generic;
using Match3.Core;

namespace Match3
{
    public class Game : IGame, IGameContext
    {
        private readonly struct FeatureInfo
        {
            public readonly IFeature Feature;
            public readonly object State;

            public FeatureInfo(IFeature feature, object state)
            {
                Feature = feature;
                State = state;
            }
        }
        
        private readonly Board _board;

        private readonly FeatureInfo[] _features;
        
        public IGameRules Rules { get; }
        public IBoard Board => _board;

        public Game(IGameRules rules, IEnumerable<ICellGridData> cellGridData)
        {
            Rules = rules;
            _board = new Board(this, cellGridData);
            _features = new FeatureInfo[rules.Features.Count];
        }

        public void Start()
        {
            var features = Rules.Features;
            var count = features.Count;
            
            for (int i = 0; i < count; ++i)
            {
                var state = features[i].Start(this);
                _features[i] = new FeatureInfo(features[i], state);
            }
        }

        public void Tick(int dTimeMs)
        {
            foreach (var featureInfo in _features)
            {
                featureInfo.Feature.Tick(this, featureInfo.State, dTimeMs);
            }
        }
    }
}