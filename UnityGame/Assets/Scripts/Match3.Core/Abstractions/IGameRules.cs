using System.Collections.Generic;

namespace Match3
{
    public interface IGameRules
    {
        IObjectFactory ObjectFactory { get; }
        IViewFactory ViewFactory { get; }
        IReadOnlyList<IFeature> Features { get; }
    }
}