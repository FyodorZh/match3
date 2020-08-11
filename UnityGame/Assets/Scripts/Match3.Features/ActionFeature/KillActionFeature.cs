using System;
using System.Collections.Generic;

namespace Match3.Features
{
    public class KillActionFeature : ActionFeature
    {
        public static readonly string Name = "Kill"; 
        
        public KillActionFeature() 
            : base(Name)
        {
        }

        public override IEnumerable<IObjectFeature> DependsOnObjectFeatures { get; } = new IObjectFeature[]
        {

        };

        public override IEnumerable<IComponentFeature> DependsOnComponentFeatures { get; } = new IComponentFeature[]
        {
            MassComponentFeature.Instance, 
        };
        
        public override void Process(IGame game, params CellId[] cells)
        {
            if (cells.Length != 1)
                throw new InvalidOperationException();

            CellId id = cells[0];

            var grid = game.Board.GetGrid(id.GridId);
            var cell = grid.GetCell(id.Position);
            
            var mass = cell.FindComponent<MassComponentFeature.IMass>();
            mass?.Owner.Release();
        }
    }
}