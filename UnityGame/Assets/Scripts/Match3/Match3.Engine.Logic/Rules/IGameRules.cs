using System.Collections.Generic;
using Match3.Features;

namespace Match3
{
    public interface IGameRules
    {
        IObjectFactory ObjectFactory { get; }
        IReadOnlyList<IGameFeature> GameFeatures { get; }
        IEnumerable<ICellComponentFeature> CellComponentFeatures { get; }

        IActionFeature FindActionFeature(string featureName);
    }
}