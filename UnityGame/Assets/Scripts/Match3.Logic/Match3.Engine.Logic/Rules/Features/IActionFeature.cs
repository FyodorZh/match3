using System.Collections.Generic;

namespace Match3.Features
{
    public interface IActionFeature : IFeature
    {
        IEnumerable<ICellComponentFeature> DependsOnCellComponentFeatures { get; }
        IEnumerable<IObjectFeature> DependsOnObjectFeatures { get; }
        IEnumerable<IObjectComponentFeature> DependsOnObjectComponentFeatures { get; }

        void Register(IGameRules rules);

        void Process(IGame game, params CellPosition[] cells);
    }
}