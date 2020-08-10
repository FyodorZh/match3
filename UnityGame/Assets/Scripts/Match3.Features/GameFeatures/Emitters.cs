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
                    if (cell.Content.Count == 1)
                    {
                        var obj = cell.Content[0];
                        var emitter = obj.TryGetComponent<EmitterComponentFeature.IEmitter>(EmitterComponentFeature.Name);
                        if (emitter != null)
                        {
                            var newObject = emitter.Emit(game);
                            if (!cell.TryAddContent(newObject))
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