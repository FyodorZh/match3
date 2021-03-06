﻿using System.Collections.Generic;
using Match3.Core;
using Match3.Features.Color;
using Match3.Features.Health;
using Match3.Features.Move;

namespace Match3.Features
{
    public sealed partial class Match : StatelessGameFeature
    {
        private static readonly DeltaTime _moveCooldown = new DeltaTime(150);

        private class ColorTable
        {
            private readonly IBoard _grid;
            private readonly Time _curTime;
            private readonly int _width;
            private readonly int _height;

            private Dictionary<CellPosition, int> _colors = new Dictionary<CellPosition, int>();

            public ColorTable(IBoard grid, Time curTime)
            {
                _grid = grid;
                _curTime = curTime;
                _width = grid.Width;
                _height = grid.Height;
            }

            public void Invalidate()
            {
                _colors.Clear();
            }

            private int GetCellColor(CellPosition pos)
            {
                if (!_colors.TryGetValue(pos, out int colorId))
                {
                    var cell = _grid.GetCell(pos);
                    IsCellEligibleForMatch(cell, _curTime, out colorId);
                    _colors.Add(pos, colorId);
                }

                return colorId;

            }

            public bool CheckPattern(Offsets2D pattern, int posX, int posY, out int colorId)
            {
                if (posX + pattern.MaxX >= _width || posY + pattern.MaxY >= _height)
                {
                    colorId = -1;
                    return false;
                }

                var pos = new CellPosition(posX, posY);

                int color0 = GetCellColor(pos + pattern.OffsetAt(0));

                if (color0 == -1)
                {
                    colorId = -1;
                    return false;
                }

                for (int i = 1; i < pattern.Length; ++i)
                {
                    var offset = pattern.OffsetAt(i);
                    int color = GetCellColor(pos + offset);
                    if (color0 != color)
                    {
                        colorId = -1;
                        return false;
                    }
                }

                colorId = color0;
                return true;
            }

            private bool IsCellEligibleForMatch(ICell cell, Time curTime, out int colorId)
            {
                if (cell.IsActive && !cell.IsLocked)
                {
                    var color = cell.FindObjectComponent<IColorCellObjectComponent>();
                    if (color != null)
                    {
                        var health = color.Owner.TryGetComponent<IHealthCellObjectComponent>();
                        if (health == null || health.HealthValue > 0)
                        {
                            IMoveCellObjectComponent move = cell.FindObjectComponent<IMoveCellObjectComponent>();
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

            var board = game.Board;
            {
                ColorTable colors = new ColorTable(board, curTime);

                int W = board.Width;
                int H = board.Height;

                List<ICell> cellList = new List<ICell>();

                foreach (var patternInfo in _patterns)
                {
                    var matchPattern = patternInfo.MatchPattern;
                    var bonusPlacement = patternInfo.BonusPlacement;

                    for (int y = H - 1; y >= 0; --y)
                    {
                        for (int x = 0; x < W; ++x)
                        {
                            var pos0 = new CellPosition(x, y);

                            int colorId = -1;

                            int matchCount = 0;
                            while (colors.CheckPattern(matchPattern, x, y, out var color) && matchCount < 10)
                            {
                                colorId = color;
                                if (matchCount == 0)
                                {
                                    Time lastChangeTime = new Time(-1);
                                    for (int i = 0; i < matchPattern.Length; ++i)
                                    {
                                        var offset = matchPattern.OffsetAt(i);
                                        var pos = pos0 + offset;

                                        var cell = board.GetCell(pos);

                                        Time time = cell.FindObjectComponent<IMoveCellObjectComponent>()?.LastMoveTime ?? Time.Zero;

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
                                }

                                for (int i = 0; i < matchPattern.Length; ++i)
                                {
                                    var offset = matchPattern.OffsetAt(i);
                                    var cell = board.GetCell(pos0 + offset);

                                    var colorComponent = cell.FindObjectComponent<IColorCellObjectComponent>();
                                    if (colorComponent != null) // только что взорвался
                                    {
                                        var colorObject = colorComponent.Owner;
                                        var health = colorObject.Owner.FindComponent<IHealthCellComponent>();
                                        health.ApplyDamage(new Damage(DamageType.Match, 1));
                                    }
                                }
                                colors.Invalidate();

                                ++matchCount;
                            }

                            Debug.Assert(matchCount < 10);

                            if (matchCount > 0)
                            {
                                var bonusData = patternInfo.BonusFactory?.Construct(colorId);
                                if (bonusData != null)
                                {
                                    var bonus = game.ObjectFactory.Construct<ICellObject>(bonusData);
                                    Debug.Assert(bonus != null);
                                    if (bonus == null)
                                    {
                                        continue;
                                    }

                                    if (cellList.Count > 2)
                                    {
                                        cellList.Clear();

                                        for (int i = 0; i < bonusPlacement.Length; ++i)
                                        {
                                            var pos = pos0 + bonusPlacement.OffsetAt(i);
                                            cellList.Add(board.GetCell(pos));
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
                                            cellList.Add(board.GetCell(pos));
                                        }

                                        game.RandomShuffle(cellList);

                                        foreach (var cell in cellList)
                                        {
                                            if (cell.CanAttach(bonus))
                                            {
                                                cell.Attach(bonus);
                                                bPlaced = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (!bPlaced)
                                    {
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
}