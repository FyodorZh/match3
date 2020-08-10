using System.Collections.Generic;

namespace Match3
{
    public interface IGameFeature : IFeature
    {
        IEnumerable<IObjectFeature> DependsOnObjectFeatures { get; }
        IEnumerable<IComponentFeature> DependsOnComponentFeatures { get; }
        
        void Register(IGameRules rules);
        
        object InitState(IGame game);
        void Tick(IGame game, object state, int dTimeMs);
    }
}