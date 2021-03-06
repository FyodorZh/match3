﻿using System;
using System.Collections.Generic;
using Match3;
using Match3.Features.Mass;
using Match3.Features.Move;

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
        };

        public override void Process(IGame game, params CellPosition[] cells)
        {
            if (cells.Length != 2)
                throw new InvalidOperationException();

            CellPosition id1 = cells[0];
            CellPosition id2 = cells[1];

            ICell cell1 = game.Board.GetCell(id1);
            ICell cell2 = game.Board.GetCell(id2);

            if (cell1 == null || cell2 == null)
                throw new InvalidOperationException();

            var mass1 = cell1.FindObjectComponent<IMassCellObjectComponent>();
            var mass2 = cell2.FindObjectComponent<IMassCellObjectComponent>();

            if (mass1 != null && !mass1.IsLocked &&
                mass2 != null && !mass2.IsLocked)
            {
                var move1 = cell1.FindObjectComponent<IMoveCellObjectComponent>();
                var move2 = cell2.FindObjectComponent<IMoveCellObjectComponent>();

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

                    move1.StartMove(new MoveCause("user"),  t1, null, () =>
                    {
                        move1.Offset = new FixedVector2(0, 0);
                        cell2.Swap(o1, o2);
                    });
                    move2.StartMove(new MoveCause("user"),  t2, null, () =>
                    {
                        move2.Offset = new FixedVector2(0, 0);
                    });
                }
            }
        }
    }
}