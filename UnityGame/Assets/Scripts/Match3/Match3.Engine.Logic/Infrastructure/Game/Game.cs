using System;
using System.Collections.Generic;
using Match3.Features;
using Match3.Utils;

namespace Match3.Logic
{
    class Game : IGame, IGameController
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
        IBoardObserver IGameObserver.Board => _board;

        public IObjectFactory ObjectFactory => Rules.ObjectFactory;

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

        public void RandomShuffle<T>(IList<T> inOutList)
        {
            int count = inOutList.Count;
            for (int i = 0; i < count; ++i)
            {
                int rnd = _random.Next() % count;

                T tmp = inOutList[i];
                inOutList[i] = inOutList[rnd];
                inOutList[rnd] = tmp;
            }
        }
    }
}