using System;
using System.Collections.Generic;
using Match3.Features;

namespace Match3.Logic
{
    class GameRules : IGameRules
    {
        private readonly List<IGameFeature> _gameFeatureList = new List<IGameFeature>();

        private readonly Dictionary<string, IGameFeature> _gameFeatures =
            new Dictionary<string, IGameFeature>();
        private readonly Dictionary<string, IActionFeature> _actionFeature =
            new Dictionary<string, IActionFeature>();
        private readonly Dictionary<string, ICellComponentFeature> _cellComponentFeatures =
            new Dictionary<string, ICellComponentFeature>();
        private readonly Dictionary<string, IObjectFeature> _objectFeatures =
            new Dictionary<string, IObjectFeature>();
        private readonly Dictionary<string, IObjectComponentFeature> _objectComponentFeatures =
            new Dictionary<string, IObjectComponentFeature>();

        private readonly ObjectFactory _objectFactory = new ObjectFactory();

        public IObjectFactory ObjectFactory => _objectFactory;

        public IReadOnlyList<IGameFeature> GameFeatures => _gameFeatureList;
        public IEnumerable<ICellComponentFeature> CellComponentFeatures => _cellComponentFeatures.Values;
        public IEnumerable<IObjectFeature> ObjectFeatures => _objectFeatures.Values;
        public IEnumerable<IObjectComponentFeature> ObjectComponentFeatures => _objectComponentFeatures.Values;

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

            foreach (var objectFeature in feature.DependsOnCellComponentFeatures)
            {
                RegisterCellComponentFeature(objectFeature);
            }

            foreach (var objectFeature in feature.DependsOnObjectFeatures)
            {
                RegisterObjectFeature(objectFeature);
            }

            foreach (var componentFeature in feature.DependsOnObjectComponentFeatures)
            {
                RegisterObjectComponentFeature(componentFeature);
            }
        }

        public void RegisterActionFeature(IActionFeature feature)
        {
            if (feature == null)
                throw new ArgumentNullException(nameof(feature));
            if (_actionFeature.ContainsKey(feature.FeatureId))
                throw new InvalidOperationException();

            _actionFeature.Add(feature.FeatureId, feature);

            foreach (var objectFeature in feature.DependsOnCellComponentFeatures)
            {
                RegisterCellComponentFeature(objectFeature);
            }

            foreach (var objectFeature in feature.DependsOnObjectFeatures)
            {
                RegisterObjectFeature(objectFeature);
            }

            foreach (var componentFeature in feature.DependsOnObjectComponentFeatures)
            {
                RegisterObjectComponentFeature(componentFeature);
            }
        }

        public void RegisterCellComponentFeature(ICellComponentFeature feature)
        {
            if (feature == null)
                throw new ArgumentNullException(nameof(feature));

            if (!_cellComponentFeatures.ContainsKey(feature.FeatureId))
            {
                _cellComponentFeatures.Add(feature.FeatureId, feature);

                foreach (var objectFeature in feature.DependsOnObjectFeatures)
                {
                    RegisterObjectFeature(objectFeature);
                }

                foreach (var componentFeature in feature.DependsOnObjectComponentFeatures)
                {
                    RegisterObjectComponentFeature(componentFeature);
                }
            }
        }

        public void RegisterObjectFeature(IObjectFeature feature)
        {
            if (feature == null)
                throw new ArgumentNullException(nameof(feature));

            if (!_objectFeatures.ContainsKey(feature.FeatureId))
            {
                _objectFeatures.Add(feature.FeatureId, feature);
                foreach (var componentFeature in feature.DependsOn)
                {
                    RegisterObjectComponentFeature(componentFeature);
                }

                _objectFactory.Append(feature.FeatureId, feature.Construct);
            }
        }

        public void RegisterObjectComponentFeature(IObjectComponentFeature feature)
        {
            if (feature == null)
                throw  new ArgumentNullException(nameof(feature));

            if (!_objectComponentFeatures.ContainsKey(feature.FeatureId))
            {
                _objectComponentFeatures.Add(feature.FeatureId, feature);
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