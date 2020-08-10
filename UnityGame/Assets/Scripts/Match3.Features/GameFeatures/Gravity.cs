using System.Collections.Generic;
using Match3.Core;

namespace Match3.Features
{
    public sealed class Gravity : GameFeature<Gravity.State>
    {
        public override IEnumerable<IObjectFeature> DependsOnObjectFeatures { get; } = new IObjectFeature[]
        {

        };
        
        public override IEnumerable<IComponentFeature> DependsOnComponentFeatures { get; } = new IComponentFeature[]
        {
            MassComponentFeature.Instance, 
        };

        public Gravity() 
            : base("Gravity")
        {
        }

        protected override State ConstructState(IGame game)
        {
            return new State();
        }

        protected override void Process(IGame game, State state, int dTimeMs)
        {
            foreach (var grid in game.Board.Grids)
            {
                for (int x = 0; x < grid.Width; ++x)
                {
                    for (int y = 1; y < grid.Height; ++y)
                    {
                        ICell cell = grid.GetCell(new CellPosition(x, y));
                        MassComponentFeature.IMass massComponent = cell.FindComponent<MassComponentFeature.IMass>();
                        if (massComponent != null && !massComponent.IsLocked)
                        {
                            var moveComponent = massComponent.Owner.TryGetComponent<MoveComponentFeature.IMove>();
                            if (moveComponent != null && !moveComponent.IsMoving)
                            {
                                int k = y - 1;
                                ICell freeCell = null;
                                while (k >= 0)
                                {
                                    freeCell = grid.GetCell(new CellPosition(x, k));
                                    if (freeCell.IsActive)
                                    {
                                        var freeMass = freeCell.FindComponent<MassComponentFeature.IMass>();
                                        if (freeMass == null)
                                        {
                                            break;
                                        }
                                    }
                                    freeCell = null;
                                    --k;
                                }

                                if (freeCell != null)
                                {
                                    var obj = massComponent.Owner;
                                    if (cell.DeattachObject(obj))
                                    {
                                        if (!freeCell.AttachObject(obj))
                                        {
                                            Debug.Assert(false);
                                            cell.AttachObject(obj);
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }


            /*
            foreach (var grid in game.Board.Grids)
            {
                for (int x = 0; x < grid.Width; ++x)
                {
                    
                    
                    
                    int emptyY = -1;
                    while (emptyY < grid.Height - 1)
                    {
                        bool bFoundEmpty = false;
                        for (int y = emptyY + 1; y < grid.Height; ++y)
                        {
                            ICell cell = grid.GetCell(new CellPosition(x, y));
                            if (cell.IsActive && cell.FindComponent<MassComponentFeature.IMass>() == null)
                            {
                                emptyY = y;
                                bFoundEmpty = true;
                                break;
                            }
                        }

                        if (bFoundEmpty)
                        {
                            for (int y = emptyY + 1; y < grid.Height; ++y)
                            {
                                ICell cell = grid.GetCell(new CellPosition(x, y));
                                if (cell.IsActive)
                                {
                                    (ICellObject cellObject, MassComponentFeature.IMass component) mass = 
                                        cell.FindObjectWithComponent<MassComponentFeature.IMass>();
                                    if (mass.component != null)
                                    {
                                        // DROP
                                        
                                        

                                        break;
                                    }
                                }
                            }
                        }
                    }

                    while (y < grid.Height && empty == null && grid)
                    {
                        
                    }
                    for (int y = 0; y < grid.Height; ++y)
                    {
                        
                    }
                }
                
                
                for (int y = 0; y < grid.Height - 1; ++y)
                {
                    for (int x = 0; x < grid.Width; ++x)
                    {
                        ICell 
                    }
                }
            }*/
        }
        
        public class State
        {
            
        }
    }
}