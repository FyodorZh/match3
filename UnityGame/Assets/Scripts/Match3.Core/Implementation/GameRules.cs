using System;
using System.Collections.Generic;

namespace Match3.Core
{
    public class GameRules : IGameRules
    {
        private readonly List<IGameFeature> _gameFeatureList = new List<IGameFeature>();

        private readonly Dictionary<string, IGameFeature> _gameFeatures = 
            new Dictionary<string, IGameFeature>();
        private readonly Dictionary<string, IObjectFeature> _objectFeatures = 
            new Dictionary<string, IObjectFeature>();
        private readonly Dictionary<string, IComponentFeature> _componentFeatures = 
            new Dictionary<string, IComponentFeature>();
        
        public IObjectFactory ObjectFactory { get; }
        
        public IViewFactory ViewFactory { get; }
        
        public IReadOnlyList<IGameFeature> GameFeatures => _gameFeatureList;

        public GameRules(IObjectFactory objectFactory, IViewFactory viewFactory)
        {
            ObjectFactory = objectFactory;
            ViewFactory = viewFactory;
        }
        
        public void RegisterGameFeature(IGameFeature feature)
        {
            if (feature == null)
                throw new ArgumentNullException(nameof(feature));
            if (_componentFeatures.ContainsKey(feature.FeatureId))
                throw new InvalidOperationException();
            
            _gameFeatures.Add(feature.FeatureId, feature);
            _gameFeatureList.Add(feature);

            foreach (var objectFeature in feature.DependsOnObjectFeatures)
            {
                RegisterObjectFeature(objectFeature);
            }

            foreach (var componentFeature in feature.DependsOnComponentFeatures)
            {
                RegisterComponentFeature(componentFeature);
            }
            
            feature.Register(this);
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
            }
        }

        public void RegisterComponentFeature(IComponentFeature feature)
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
            // TODO
        }
    }
}