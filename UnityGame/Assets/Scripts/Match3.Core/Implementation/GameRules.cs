using System;
using System.Collections.Generic;

namespace Match3.Core
{
    public class GameRules : IGameRules
    {
        private readonly List<IGameFeature> _gameFeatureList = new List<IGameFeature>();

        private readonly Dictionary<string, IGameFeature> _gameFeatures =
            new Dictionary<string, IGameFeature>();
        private readonly Dictionary<string, IActionFeature> _actionFeature =
            new Dictionary<string, IActionFeature>();
        private readonly Dictionary<string, IObjectFeature> _objectFeatures =
            new Dictionary<string, IObjectFeature>();
        private readonly Dictionary<string, IObjectComponentFeature> _componentFeatures =
            new Dictionary<string, IObjectComponentFeature>();

        private readonly ObjectFactory _objectFactory = new ObjectFactory();

        public IObjectFactory ObjectFactory => _objectFactory;

        public IViewFactory ViewFactory { get; }

        public IReadOnlyList<IGameFeature> GameFeatures => _gameFeatureList;

        public GameRules(IViewFactory viewFactory)
        {
            ViewFactory = viewFactory;
        }

        public IActionFeature FindActionFeature(string featureName)
        {
            _actionFeature.TryGetValue(featureName, out var result);
            return result;
        }

        public void RegisterGameFeature(IGameFeature feature)
        {
            if (feature == null)
                throw new ArgumentNullException(nameof(feature));
            if (_gameFeatures.ContainsKey(feature.FeatureId))
                throw new InvalidOperationException();

            _gameFeatures.Add(feature.FeatureId, feature);
            _gameFeatureList.Add(feature);

            foreach (var objectFeature in feature.DependsOnObjectFeatures)
            {
                RegisterObjectFeature(objectFeature);
            }

            foreach (var componentFeature in feature.DependsOnObjectComponentFeatures)
            {
                RegisterComponentFeature(componentFeature);
            }
        }

        public void RegisterActionFeature(IActionFeature feature)
        {
            if (feature == null)
                throw new ArgumentNullException(nameof(feature));
            if (_actionFeature.ContainsKey(feature.FeatureId))
                throw new InvalidOperationException();

            _actionFeature.Add(feature.FeatureId, feature);

            foreach (var objectFeature in feature.DependsOnObjectFeatures)
            {
                RegisterObjectFeature(objectFeature);
            }

            foreach (var componentFeature in feature.DependsOnComponentFeatures)
            {
                RegisterComponentFeature(componentFeature);
            }
        }

        public void RegisterObjectFeature(IObjectFeature feature)
        {
            if (feature == null)
                throw  new ArgumentNullException(nameof(feature));

            if (!_objectFeatures.ContainsKey(feature.FeatureId))
            {
                _objectFeatures.Add(feature.FeatureId, feature);
                foreach (var componentFeature in feature.DependsOn)
                {
                    RegisterComponentFeature(componentFeature);
                }

                _objectFactory.Append(feature.FeatureId, (data, context) => feature.Construct(data));
            }
        }

        public void RegisterComponentFeature(IObjectComponentFeature feature)
        {
            if (feature == null)
                throw  new ArgumentNullException(nameof(feature));

            if (!_componentFeatures.ContainsKey(feature.FeatureId))
            {
                _componentFeatures.Add(feature.FeatureId, feature);
            }
        }

        public void BakeAllFeatures()
        {
            foreach (var feature in _gameFeatureList)
            {
                feature.Register(this);
            }
        }
    }
}