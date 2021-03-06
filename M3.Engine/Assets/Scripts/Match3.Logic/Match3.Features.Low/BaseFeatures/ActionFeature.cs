using System.Collections.Generic;

namespace Match3.Features
{
    public abstract class ActionFeature : IActionFeature
    {
        protected IGameRules Rules { get; private set; }

        public string FeatureId { get; }


        protected ActionFeature(string featureName)
        {
            FeatureId = featureName;
        }

        public abstract IEnumerable<ICellComponentFeature> DependsOnCellComponentFeatures { get; }
        public abstract IEnumerable<IObjectFeature> DependsOnObjectFeatures { get; }
        public abstract IEnumerable<IObjectComponentFeature> DependsOnObjectComponentFeatures { get; }

        public void Register(IGameRules rules)
        {
            Rules = rules;
        }

        public abstract void Process(IGame game,params CellPosition[] cells);
    }
}