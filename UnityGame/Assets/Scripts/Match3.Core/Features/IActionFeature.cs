using System.Collections.Generic;

namespace Match3
{
    public interface IActionFeature : IFeature
    {
        IEnumerable<IObjectFeature> DependsOnObjectFeatures { get; }
        IEnumerable<IObjectComponentFeature> DependsOnComponentFeatures { get; }
        
        void Register(IGameRules rules);

        void Process(IGame game, params CellId[] cells);
    }
}