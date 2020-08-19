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
            HealthCellComponentFeature.Instance,
        };

        public override IEnumerable<IObjectFeature> DependsOnObjectFeatures { get; } = new IObjectFeature[]
        {
        };

        public override IEnumerable<IObjectComponentFeature> DependsOnObjectComponentFeatures { get; } = new IObjectComponentFeature[]
        {
        };

        public override void Process(IGame game, params CellId[] cells)
        {
            if (cells.Length != 1)
                throw new InvalidOperationException();

            CellId id = cells[0];

            var cell = game.Board.GetCell(id);

            var healthComponent = cell.FindComponent<HealthCellComponentFeature.IHealth>();
            healthComponent?.ApplyDamage(new Damage(DamageType.Match, 1));
        }
    }
}