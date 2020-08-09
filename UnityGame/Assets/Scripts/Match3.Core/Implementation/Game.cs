using System.Collections.Generic;
using Match3.Core;

namespace Match3
{
    public class Game : IGame, IGameContext
    {
        private readonly Board _board;
        
        public IGameRules Rules { get; }
        public IBoard Board => _board;

        public Game(IGameRules rules, IEnumerable<ICellGridData> cellGridData)
        {
            Rules = rules;
            _board = new Board(this, cellGridData);
        }

        public void Start()
        {
            var features = Rules.Features;
            var count = features.Count;
            for (int i = 0; i < count; ++i)
            {
                features[i].Start(this);
            }
        }

        public void Tick(int dTimeMs)
        {
            var features = Rules.Features;
            var count = features.Count;
            for (int i = 0; i < count; ++i)
            {
                features[i].Tick(this);
            }
        }
    }
}