using System.Collections.Generic;
using Match3.Core;

namespace Match3
{
    public class Game : IGame, IGameContext
    {
        private readonly struct FeatureInfo
        {
            public readonly IGameFeature Feature;
            public readonly object State;

            public FeatureInfo(IGameFeature feature, object state)
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
            _features = new FeatureInfo[rules.GameFeatures.Count];
        }

        public void Start()
        {
            var features = Rules.GameFeatures;
            var count = features.Count;
            
            for (int i = 0; i < count; ++i)
            {
                var state = features[i].InitState(this);
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