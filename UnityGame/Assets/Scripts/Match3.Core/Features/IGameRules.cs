using System.Collections.Generic;

namespace Match3
{
    public interface IGameRules
    {
        IObjectFactory ObjectFactory { get; }
        IViewFactory ViewFactory { get; }
        IReadOnlyList<IGameFeature> GameFeatures { get; }

        void RegisterGameFeature(IGameFeature feature);
        void RegisterObjectFeature(IObjectFeature feature);
        void RegisterComponentFeature(IComponentFeature feature);
        
        void BakeAllFeatures();
    }
}