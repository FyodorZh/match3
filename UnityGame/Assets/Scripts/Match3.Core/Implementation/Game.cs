using System;
using System.Collections.Generic;
using Match3.Core;
using Match3;

namespace Match3
{
    public class Game : IGame, IGameController
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

        private readonly List<ActionInfo> _externalActions = new List<ActionInfo>();

        private readonly ActionStream _internalActions = new ActionStream();

        private readonly Random _random = new Random(777);

        private Time _currentTime;

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
            foreach (var feature in Rules.CellComponentFeatures)
            {
                feature.InitState(this);
            }

            var features = Rules.GameFeatures;
            var count = features.Count;

            for (int i = 0; i < count; ++i)
            {
                var state = features[i].InitState(this);
                _features[i] = new FeatureInfo(features[i], state);
            }
        }

        public void Tick(DeltaTime dTime)
        {
            _currentTime += dTime;

            foreach (var action in _externalActions)
            {
                action.ActionFeature.Process(this, action.Cells);
            }
            _externalActions.Clear();
            _internalActions.Process();

            _board.Tick(dTime);
            _internalActions.Process();

            foreach (var featureInfo in _features)
            {
                featureInfo.Feature.Tick(this, featureInfo.State, dTime);
            }
            _internalActions.Process();
        }

        public void Action(string actionFeatureName, params CellId[] cells)
        {
            if (actionFeatureName == null)
                throw new ArgumentException(nameof(actionFeatureName));

            if (cells == null)
                throw new ArgumentException(nameof(cells));

            var feature = Rules.FindActionFeature(actionFeatureName);
            if (feature == null)
                throw new InvalidOperationException();

            _externalActions.Add(new ActionInfo(feature, cells));
        }

        public void InternalInvoke(Action action)
        {
            _internalActions.Put(action);
        }

        private readonly struct ActionInfo
        {
            public readonly IActionFeature ActionFeature;
            public readonly CellId[] Cells;

            public ActionInfo(IActionFeature actionFeature, CellId[] cells)
            {
                ActionFeature = actionFeature;
                Cells = cells;
            }
        }

        public Time CurrentTime => _currentTime;

        public int GetRandom()
        {
            return _random.Next();
        }
    }
}