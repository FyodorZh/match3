using System.Collections.Generic;

namespace Match3.Features
{
    public interface ICellComponentFeature : IFeature
    {
        IEnumerable<IObjectFeature> DependsOnObjectFeatures { get; }
        IEnumerable<IObjectComponentFeature> DependsOnObjectComponentFeatures { get; }

        void InitState(IGame game);
    }
}