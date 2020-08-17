using System.Collections.Generic;
using Match3.Core;
using Match3.Features.CellComponentFeatures;

namespace Match3.Features
{
    public sealed class Match : StatelessGameFeature
    {
        public override IEnumerable<ICellComponentFeature> DependsOnCellComponentFeatures { get; } = new ICellComponentFeature[]
        {
        };

        public override IEnumerable<IObjectFeature> DependsOnObjectFeatures { get; } = new IObjectFeature[]
        {
        };

        public override IEnumerable<IObjectComponentFeature> DependsOnObjectComponentFeatures { get; } = new IObjectComponentFeature[]
        {
            ColorObjectComponentFeature.Instance,
        };

        public Match()
            : base("Match")
        {
        }

        protected override void Process(IGame game, int dTimeMs)
        {
            int curTime = game.CurrentTime;

            foreach (var grid in game.Board.Grids)
            {
                int[,] colors = new int[grid.Width, grid.Height];

                foreach (var cell in grid.AllCells)
                {
                    if (cell.IsActive && !cell.IsLocked)
                    {
                        var color = cell.FindObjectComponent<ColorObjectComponentFeature.IColor>();
                        if (color != null)
                        {
                            MoveObjectComponentFeature.IMove move = cell.FindObjectComponent<MoveObjectComponentFeature.IMove>();
                            if (move == null || move.MoveCause.Value == "user" || curTime - move.LastMoveTime > 150)
                            {
                                colors[cell.Position.X, cell.Position.Y] = color.ColorId;
                                continue;
                            }
                        }
                    }

                    colors[cell.Position.X, cell.Position.Y] = -1;
                }

                int W = grid.Width;
                int H = grid.Height;
                for (int x = 0; x < W; ++x)
                {
                    for (int y = 0; y < H; ++y)
                    {
                        var color = colors[x, y];
                        if (color == -1)
                        {
                            continue;
                        }

                        foreach (var pattern in _patterns)
                        {
                            if (x + pattern.Width <= W && y + pattern.Height <= H)
                            {
                                bool isOK = true;
                                for (int i = 1; i < pattern.OffsetsX.Length; ++i)
                                {
                                    if (colors[x + pattern.OffsetsX[i], y + pattern.OffsetsY[i]] != color)
                                    {
                                        isOK = false;
                                        break;
                                    }
                                }

                                if (isOK)
                                {
                                    for (int i = 0; i < pattern.OffsetsX.Length; ++i)
                                    {
                                        colors[x + pattern.OffsetsX[i], y + pattern.OffsetsY[i]] = -1;
                                        var cell = grid.GetCell(new CellPosition(x + pattern.OffsetsX[i], y + pattern.OffsetsY[i]));
                                        var colorComponent = cell.FindObjectComponent<ColorObjectComponentFeature.IColor>();
                                        var colorObject = colorComponent.Owner;
                                        var health = colorObject.Owner.FindComponent<HealthCellComponentFeature.IHealth>();
                                        health.ApplyDamage(new Damage(DamageType.Match, 1));
                                        //game.InternalInvoke(() => colorObject.Release());
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }


        private readonly Pattern[] _patterns = new Pattern[]
        {
            new Pattern(new int[][]{new[]{0,0}, new[]{0,1}, new[]{0, 2}, new[]{0,3}, new[]{0,4}}),
            new Pattern(new int[][]{new[]{0,0}, new[]{1,0}, new[]{2, 0}, new[]{3,0}, new[]{4,0}}),
            new Pattern(new int[][]{new[]{0,0}, new[]{0,1}, new[]{0, 2}, new[]{0,3}}),
            new Pattern(new int[][]{new[]{0,0}, new[]{1,0}, new[]{2, 0}, new[]{3,0}}),
            new Pattern(new int[][]{new[]{0,0}, new[]{0,1}, new[]{0, 2}}),
            new Pattern(new int[][]{new[]{0,0}, new[]{1,0}, new[]{2, 0}}),

            new Pattern(new int[][]{new[]{0,0}, new[]{1,0}, new[]{0, 1}, new[]{1, 1}}),
        };

        private class Pattern
        {
            public readonly int Length;
            public readonly int Width;
            public readonly int Height;
            public readonly int[] OffsetsX;
            public readonly int[] OffsetsY;

            public Pattern(int[][] list)
            {
                Length = list.Length;
                OffsetsX = new int[Length];
                OffsetsY = new int[Length];
                for (int i = 0; i < Length; ++i)
                {
                    if (Width <= list[i][0])
                        Width = list[i][0] + 1;
                    if (Height <= list[i][1])
                        Height = list[i][1] + 1;

                    OffsetsX[i] = list[i][0];
                    OffsetsY[i] = list[i][1];
                }
            }
        }
    }
}