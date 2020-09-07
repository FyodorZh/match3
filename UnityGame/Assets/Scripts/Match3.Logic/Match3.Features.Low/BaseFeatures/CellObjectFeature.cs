using System.Collections.Generic;

namespace Match3.Features
{
    public abstract class CellObjectFeature : ICellObjectFeature
    {
        public abstract string FeatureId { get; }

        public abstract void Init(IGameRules rules);

        public abstract IObject Construct(IObjectData data);
    }
}