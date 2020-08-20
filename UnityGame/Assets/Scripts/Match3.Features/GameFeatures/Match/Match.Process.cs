using System.Collections.Generic;
using Match3.Core;

namespace Match3.Features
{
    public sealed partial class Match : StatelessGameFeature
    {
        private static readonly DeltaTime _moveCooldown = new DeltaTime(150);

        private class ColorTable
        {
            private readonly IGrid _grid;
            private readonly int _width;
            private readonly int _height;

            private readonly int[][] _colors;

            public ColorTable(IGrid grid)
            {
                _grid = grid;
                _width = grid.Width;
                _height = grid.Height;

                _colors = new int[_height][];
                for (int i = 0; i < _height; ++i)
                {
                    _colors[i] = new int[_width];
                }
            }

            public void RefreshAllColors(Time curTime)
            {
                foreach (var cell in _grid.AllCells)
                {
                    IsCellEligibleForMatch(cell, curTime, out var colorId);
                    var pos = cell.Position;
                    _colors[pos.Y][pos.X] = colorId;
                }
            }

            public bool CheckPattern(Offsets2D pattern, int posX, int posY, out int colorId)
            {
                if (posX + pattern.MaxX >= _width || posY + pattern.MaxY >= _height)
                {
                    colorId = -1;
                    return false;
                }

                var pos0 = pattern.OffsetAt(0);
                int color0 = _colors[posY + pos0.Y][posX + pos0.X];

                if (color0 == -1)
                {
                    colorId = -1;
                    return false;
                }

                for (int i = 1; i < pattern.Length; ++i)
                {
                    var pos = pattern.OffsetAt(i);
                    int color = _colors[posY + pos.Y][posX + pos.X];
                    if (color0 != color)
                    {
                        colorId = -1;
                        return false;
                    }
                }

                colorId = color0;
                return true;
            }

            public void RefreshColorsOfPattern(Time curTime, Offsets2D pattern, int posX, int posY)
            {
                CellPosition pos = new CellPosition(posX, posY);
                for (int i = 0; i < pattern.Length; ++i)
                {
                    var offset = pattern.OffsetAt(i);
                    pos += offset;

                    var cell = _grid.GetCell(pos);

                    IsCellEligibleForMatch(cell, curTime, out var colorId);
                    _colors[pos.Y][pos.X] = colorId;
                }
            }

            private bool IsCellEligibleForMatch(ICell cell, Time curTime, out int colorId)
            {
                if (cell.IsActive && !cell.IsLocked)
                {
                    var color = cell.FindObjectComponent<ColorObjectComponentFeature.IColor>();
                    if (color != null)
                    {
                        var health = color.Owner.TryGetComponent<HealthObjectComponentFeature.IHealth>();
                        if (health == null || health.HealthValue > 0)
                        {
                            MoveObjectComponentFeature.IMove move = cell.FindObjectComponent<MoveObjectComponentFeature.IMove>();
                            if (move == null || move.MoveCause.Value == "user" || curTime - move.LastMoveTime > _moveCooldown)
                            {
                                colorId = color.ColorId;
                                return true;
                            }
                        }
                    }
                }

                colorId = -1;
                return false;
            }
        }

        protected override void Process(IGame game, DeltaTime dTime)
        {
            var curTime = game.CurrentTime;

            foreach (var grid in game.Board.Grids)
            {
                ColorTable colors = new ColorTable(grid);

                colors.RefreshAllColors(curTime);


                int W = grid.Width;
                int H = grid.Height;
                for (int y = H - 1; y >= 0; --y)
                {
                    for (int x = 0; x < W; ++x)
                    {
                        foreach (var patternInfo in _patterns)
                        {
                            var matchPattern = patternInfo.MatchPattern;

                            int colorId;

                            int matchCount = 0;
                            while (colors.CheckPattern(matchPattern, x, y, out colorId) && matchCount < 10)
                            {
                                for (int i = 0; i < matchPattern.Length; ++i)
                                {
                                    var offset = matchPattern.OffsetAt(i);

                                    var cell = grid.GetCell(new CellPosition(x + offset.X, y + offset.Y));
                                    var colorComponent = cell.FindObjectComponent<ColorObjectComponentFeature.IColor>();
                                    var colorObject = colorComponent.Owner;
                                    var health = colorObject.Owner.FindComponent<HealthCellComponentFeature.IHealth>();
                                    health.ApplyDamage(new Damage(DamageType.Match, 1));
                                }

                                colors.RefreshColorsOfPattern(curTime, matchPattern, x, y);

                                ++matchCount;
                            }

                            Debug.Assert(matchCount < 10);

                            if (matchCount > 0)
                            {
                                var bonusData = patternInfo.BonusFactory?.Construct(colorId);
                                if (bonusData != null)
                                {
                                    List<ICell> cellList = new List<ICell>();
                                    Time lastChangeTime = new Time(-1);

                                    var pos0 = new CellPosition(x, y);
                                    for (int i = 0; i < matchPattern.Length; ++i)
                                    {
                                        var offset = matchPattern.OffsetAt(i);
                                        var pos = pos0 + offset;

                                        var cell = grid.GetCell(pos);

                                        Time time = cell.FindObjectComponent<MoveObjectComponentFeature.IMove>()?.LastMoveTime ?? Time.Zero;

                                        if (time > lastChangeTime)
                                        {
                                            cellList.Clear();
                                            cellList.Add(cell);
                                            lastChangeTime = time;
                                        }
                                        else if (time == lastChangeTime)
                                        {
                                            cellList.Add(cell);
                                        }
                                    }

                                    Debug.Assert(cellList.Count > 0);

                                    var bonus = game.Rules.ObjectFactory.Construct<ICellObject>(bonusData, game);
                                    Debug.Assert(bonus != null);
                                    if (bonus == null)
                                        break;

                                    if (cellList.Count > 2)
                                    {
                                        cellList.Clear();

                                        var bonusPlacement = patternInfo.BonusPlacement;
                                        for (int i = 0; i < bonusPlacement.Length; ++i)
                                        {
                                            var pos = pos0 + bonusPlacement.OffsetAt(i);
                                            cellList.Add(grid.GetCell(pos));
                                        }
                                    }

                                    game.RandomShuffle(cellList);

                                    bool bPlaced = false;
                                    for (int i = 0; i < cellList.Count; ++i)
                                    {
                                        if (cellList[i].CanAttach(bonus))
                                        {
                                            cellList[i].Attach(bonus);
                                            bPlaced = true;
                                            break;
                                        }
                                    }

                                    if (!bPlaced)
                                    {
                                        cellList.Clear();

                                        for (int i = 0; i < matchPattern.Length; ++i)
                                        {
                                            var pos = pos0 + matchPattern.OffsetAt(i);
                                            cellList.Add(grid.GetCell(pos));
                                        }

                                        game.RandomShuffle(cellList);

                                        foreach (var cell in cellList)
                                        {
                                            if (cell.CanAttach(bonus))
                                            {
                                                cell.Attach(bonus);
                                                break;
                                            }
                                        }
                                    }

                                    bonus.Release();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}