using System.Collections.Generic;
using Match3.Core;

namespace Match3
{
    public class Game : IGame, IGameContext
    {
        private readonly IGameRules _rules;

        private readonly Board _board;
        
        public Game(IGameRules rules, IEnumerable<ICellGridData> cellGridData)
        {
            _rules = rules;
            _board = new Board(cellGridData);
        }
    }
}