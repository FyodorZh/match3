using System.Collections.Generic;
using Match3.Core;
using Match3.Math;

namespace Match3
{
    public class Game : IGame
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
        
        internal Board Board => _board;
        
        public IGameRules Rules { get; }
        IBoard IGame.Board => _board;

        public Game(IGameRules rules, IEnumerable<IGridData> cellGridData)
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
            Fixed fixedTimeSeconds = new Fixed(dTimeMs, 1000);
            _board.Tick(fixedTimeSeconds);
            foreach (var featureInfo in _features)
            {
                featureInfo.Feature.Tick(this, featureInfo.State, dTimeMs);
            }
        }
    }
}