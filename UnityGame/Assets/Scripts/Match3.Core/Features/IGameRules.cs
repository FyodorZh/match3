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
        void RegisterCellComponentFeature(ICellComponentFeature feature);
        void RegisterObjectFeature(IObjectFeature feature);
        //void RegisterObjectComponentFeature(IObjectComponentFeature feature);

        void BakeAllFeatures();
    }
}