using System.Collections.Generic;
using Match3.Math;

namespace Match3.Features
{
    public sealed class Movements : GameFeature<Movements.State>
    {
        public override IEnumerable<IObjectFeature> DependsOnObjectFeatures { get; } = new IObjectFeature[]
        {

        };
        
        public override IEnumerable<IComponentFeature> DependsOnComponentFeatures { get; } = new IComponentFeature[]
        {
            MoveComponentFeature.Instance, 
        };

        public Movements() 
            : base("Movements")
        {
        }

        protected override State ConstructState(IGame game)
        {
            return new State();
        }

        protected override void Process(IGame game, State state, int dTimeMs)
        {
            Fixed dTime = new Fixed(dTimeMs, 1000);
            foreach (var grid in game.Board.Grids)
            {
                foreach (var cell in grid.AllCells)
                {
                    var moveComponent = cell.FindComponent<MoveComponentFeature.IMove>();
                    if (moveComponent != null && moveComponent.IsMoving)
                    {
                        moveComponent.Update(dTime);
                    }
                }
            }
        }
        
        public class State
        {
        }
    }
}