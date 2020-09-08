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

        public override IEnumerable<ICellComponentFeature> DependsOnCellComponentFeatures { get; } = new ICellComponentFeature[]
        {
        };

        public override IEnumerable<IObjectFeature> DependsOnObjectFeatures { get; } = new IObjectFeature[]
        {
        };

        public override IEnumerable<IObjectComponentFeature> DependsOnObjectComponentFeatures { get; } = new IObjectComponentFeature[]
        {
        };

        public override void Process(IGame game, params CellPosition[] cells)
        {
            if (cells.Length != 1)
                throw new InvalidOperationException();

            CellPosition pos = cells[0];

            var cell = game.Board.GetCell(pos);

            var healthComponent = cell.FindComponent<IHealthCellComponent>();
            healthComponent?.ApplyDamage(new Damage(DamageType.Match, 1));
        }
    }
}