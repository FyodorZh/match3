using System;
using System.Collections.Generic;
using Match3.Math;

namespace Match3.Features
{
    public class SwapActionFeature : ActionFeature
    {
        public static readonly string Name = "Swap";

        public SwapActionFeature()
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
            MassObjectComponentFeature.Instance,
            MoveObjectComponentFeature.Instance,
        };

        public override void Process(IGame game, params CellId[] cells)
        {
            if (cells.Length != 2)
                throw new InvalidOperationException();

            CellId id1 = cells[0];
            CellId id2 = cells[1];

            ICell cell1 = game.Board.GetCell(id1);
            ICell cell2 = game.Board.GetCell(id2);

            if (cell1 == null || cell2 == null)
                throw new InvalidOperationException();

            var mass1 = cell1.FindObjectComponent<MassObjectComponentFeature.IMass>();
            var mass2 = cell2.FindObjectComponent<MassObjectComponentFeature.IMass>();

            if (mass1 != null && !mass1.IsLocked &&
                mass2 != null && !mass2.IsLocked)
            {
                var move1 = cell1.FindObjectComponent<MoveObjectComponentFeature.IMove>();
                var move2 = cell2.FindObjectComponent<MoveObjectComponentFeature.IMove>();

                if (move1 != null && !move1.IsMoving &&
                    move2 != null && !move2.IsMoving)
                {
                    FixedVector2 pos1 = new FixedVector2(cell1.Position.X, cell1.Position.Y);
                    FixedVector2 pos2 = new FixedVector2(cell2.Position.X, cell2.Position.Y);

                    FixedVector2 delta = pos2 - pos1;

                    var t1 = new LineTrajectory(new FixedVector2(0, 0), delta, new Fixed(1, 4), LineTrajectory.Linear);
                    var t2 = new LineTrajectory(new FixedVector2(0, 0), -delta, new Fixed(1, 4), LineTrajectory.Linear);

                    var o1 = move1.Owner;
                    var o2 = move2.Owner;

                    move1.SetTrajectory(t1, null, () => cell2.Attach(o1));
                    move2.SetTrajectory(t2, null, () => cell1.Attach(o2));
                }
            }
        }
    }
}