using System.Collections.Generic;

namespace Match3
{
    public interface ICellComponentFeature : IFeature
    {
        IEnumerable<IObjectFeature> DependsOnObjectFeatures { get; }
        IEnumerable<IObjectComponentFeature> DependsOnObjectComponentFeatures { get; }

        void Register(IGameRules rules);
    }
}