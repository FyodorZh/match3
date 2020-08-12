using System.Collections.Generic;
using Match3.Core;

namespace Match3.Features
{
    public sealed class Emitters : StatelessGameFeature
    {
        public override IEnumerable<IObjectFeature> DependsOnObjectFeatures { get; } = new IObjectFeature[]
        {
            EmitterObjectFeature.Instance, 
        };
        
        public override IEnumerable<IComponentFeature> DependsOnComponentFeatures { get; } = new IComponentFeature[]
        {
            EmitterComponentFeature.Instance, 
        };
        
        public Emitters() 
            : base("Emitters")
        {
        }
        
        protected override void Process(IGame game, int dTimeMs)
        {
            foreach (var grid in game.Board.Grids)
            {
                foreach (var cell in grid.AllCells)
                {
                    if (cell.Objects.Count == 1)
                    {
                        var obj = cell.Objects[0];
                        var emitter = obj.TryGetComponent<EmitterComponentFeature.IEmitter>();
                        if (emitter != null)
                        {
                            if (cell.IsActive && !cell.IsLocked)
                            {
                                var cellUnder = cell.Under();
                                Debug.Assert(cellUnder != null && cellUnder.IsActive);
                                if (!cellUnder.IsLocked)
                                {
                                    var newObject = emitter.Emit(game);
                                    if (newObject != null)
                                    {
                                        if (!cell.CanAttach(newObject) || !cell.Attach(newObject))
                                        {
                                            Debug.Assert(false);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}