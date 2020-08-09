using System.Collections.Generic;
using Match3.Core;

namespace Match3
{
    public class Game : IGame, IGameContext
    {
        private readonly Board _board;
        
        public IGameRules Rules { get; }
        
        public Game(IGameRules rules, IEnumerable<ICellGridData> cellGridData)
        {
            Rules = rules;
            _board = new Board(this, cellGridData);
        }

        public void Tick(int dTimeMs)
        {
            // TODO
        }
    }
}