﻿using System.Collections.Generic;
using Match3;

public class TrivialBoardData : IBoardData
{
    private readonly List<ICellObjectData>[,] _cells;

    public int PosX => 0;
    public int PosY => 0;

    public int Width { get; }
    public int Height { get; }

    public TrivialBoardData(int with, int height)
    {
        Width = with;
        Height = height;
        _cells = new List<ICellObjectData>[Width, Height];
    }

    public void ActivateCell(int x, int y)
    {
        if (_cells[x, y] == null)
        {
            _cells[x, y] = new List<ICellObjectData>();
        }
    }

    public void AddCellContent(int x, int y, ICellObjectData data)
    {
        ActivateCell(x, y);
        _cells[x, y].Add(data);
    }

    public IEnumerable<ICellObjectData> GetCellDataAt(int x, int y)
    {
        return _cells[x, y];
    }

    public IBorderObjectData GetBorderDataAt(int x, int y)
    {
        return null;
    }
}