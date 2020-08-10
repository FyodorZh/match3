using System.Collections.Generic;

namespace Match3
{
    public interface IObjectFeature : IFeature
    {
        IEnumerable<IComponentFeature> DependsOn { get; }

        IObject Construct();
    }
}