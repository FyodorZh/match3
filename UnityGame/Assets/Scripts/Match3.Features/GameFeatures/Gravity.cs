using System.Collections.Generic;

namespace Match3.Features
{
    public sealed class Gravity : GameFeature<Gravity.State>
    {
        public override IEnumerable<IObjectFeature> DependsOnObjectFeatures { get; } = new IObjectFeature[]
        {

        };
        
        public override IEnumerable<IComponentFeature> DependsOnComponentFeatures { get; } = new IComponentFeature[]
        {
            
        };

        public Gravity() 
            : base("Gravity")
        {
        }

        protected override State ConstructData()
        {
            return new State();
        }

        protected override void Process(IGame game, State state, int dTimeMs)
        {
        }
        
        public class State
        {
            
        }
    }
}