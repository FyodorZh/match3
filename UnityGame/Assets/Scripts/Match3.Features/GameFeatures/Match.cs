using System;
using System.Collections.Generic;
using Match3.Core;

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

        protected override void Process(IGame game, DeltaTime dTime)
        {
            DeltaTime moveCooldown = new DeltaTime(150);

            var curTime = game.CurrentTime;

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
                            if (move == null || move.MoveCause.Value == "user" || curTime - move.LastMoveTime > moveCooldown)
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

                        foreach (var patternMatch in _patterns)
                        {
                            var pattern = patternMatch.Match;
                            var bonus = patternMatch.Bonus;

                            if (x + pattern.MaxX < W && y + pattern.MaxY < H)
                            {
                                bool isOK = true;
                                for (int i = 0; i < pattern.Length; ++i)
                                {
                                    var offset = pattern.OffsetAt(i);
                                    if (colors[x + offset.X, y + offset.Y] != color)
                                    {
                                        isOK = false;
                                        break;
                                    }
                                }

                                if (isOK)
                                {
                                    for (int i = 0; i < pattern.Length; ++i)
                                    {
                                        var offset = pattern.OffsetAt(i);

                                        colors[x + offset.X, y + offset.Y] = -1;
                                        var cell = grid.GetCell(new CellPosition(x + offset.X, y + offset.Y));
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

        private readonly List<MatchPattern> _patterns = new List<MatchPattern>();
        private readonly HashSet<Offsets2D> _patternSet = new HashSet<Offsets2D>();

        public void RegisterPatterns(IObjectData bonus, params Offsets2D[] patterns)
        {
            foreach (var pattern in patterns)
            {
                if (_patternSet.Contains(pattern))
                {
                    throw new InvalidOperationException();
                }

                var p = pattern;
                for (int i = 0; i < 4; ++i)
                {
                    if (_patternSet.Add(p))
                    {
                        _patterns.Add(new MatchPattern(p, bonus));
                    }

                    p = p.RotateRight();
                    p.OffsetPivot(p.MinX, p.MinY);
                }
            }
        }

        private class MatchPattern
        {
            public readonly Offsets2D Match;
            public readonly IObjectData Bonus;

            public MatchPattern(Offsets2D matchPattern, IObjectData bonus = null)
            {
                Match = matchPattern;
                Bonus = bonus;
            }
        }
    }
}