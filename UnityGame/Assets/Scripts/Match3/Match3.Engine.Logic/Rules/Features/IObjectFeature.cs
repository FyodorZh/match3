using System.Collections.Generic;

namespace Match3.Features
{
    public interface IObjectFeature : IFeature
    {
        IEnumerable<IObjectComponentFeature> DependsOn { get; }

        IObject Construct(IObjectData data);
    }
}