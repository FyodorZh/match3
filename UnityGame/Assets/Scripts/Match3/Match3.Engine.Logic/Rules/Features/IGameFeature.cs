using System.Collections.Generic;

namespace Match3.Features
{
    public interface IGameFeature : IFeature
    {
        IEnumerable<ICellComponentFeature> DependsOnCellComponentFeatures { get; }
        IEnumerable<IObjectFeature> DependsOnObjectFeatures { get; }
        IEnumerable<IObjectComponentFeature> DependsOnObjectComponentFeatures { get; }

        void Register(IGameRules rules);

        object InitState(IGame game);
        void Tick(IGame game, object state, DeltaTime dTime);
    }
}