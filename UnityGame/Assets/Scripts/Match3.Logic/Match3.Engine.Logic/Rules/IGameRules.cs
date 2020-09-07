using System.Collections.Generic;
using Match3.Features;

namespace Match3
{
    public interface IGameRules
    {
        IObjectFactory ObjectFactory { get; }

        IReadOnlyList<IGameFeature> GameFeatures { get; }
        IEnumerable<ICellComponentFeature> CellComponentFeatures { get; }

        IEnumerable<IObjectFeature> ObjectFeatures { get; }

        IEnumerable<IObjectComponentFeature> ObjectComponentFeatures { get; }

        IActionFeature FindActionFeature(string featureName);

        TCellObjectComponentFeature GetCellObjectComponentFeature<TCellObjectComponentFeature>(string name)
            where TCellObjectComponentFeature : ICellObjectComponentFeature;
    }
}