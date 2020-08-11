using System.Collections.Generic;

namespace Match3
{
    public interface IGameRules
    {
        IObjectFactory ObjectFactory { get; }
        IViewFactory ViewFactory { get; }
        IReadOnlyList<IGameFeature> GameFeatures { get; }

        IActionFeature FindActionFeature(string featureName);

        void RegisterGameFeature(IGameFeature feature);
        void RegisterActionFeature(IActionFeature feature);
        void RegisterObjectFeature(IObjectFeature feature);
        void RegisterComponentFeature(IComponentFeature feature);
        
        void BakeAllFeatures();
    }
}