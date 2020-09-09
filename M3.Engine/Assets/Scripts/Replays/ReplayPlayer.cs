using System;
using Match3;

namespace Replays
{
    public class ReplayPlayer : IGameController
    {
        private ReplayData _data;
        private IGameController _controller;

        private int _pos = 0;

        public ReplayData Data => _data;

        public int TickId => Math.Min(_pos, _data.Size);

        public ReplayPlayer(ReplayData data, IGameController controller)
        {
            _data = data;
            _controller = controller;
        }

        public void Start()
        {
            _data.Run(_pos++, _controller);
        }

        public void Tick(DeltaTime dTime)
        {
            _data.Run(_pos++, _controller);
        }

        public void Action(string actionFeatureName, params CellPosition[] cells)
        {
        }

        public void Restart()
        {
            _pos = 0;
        }

        public void ApplyNSteps(int n)
        {
            for (int i = 0; i < n; ++i)
            {
                _data.Run(_pos++, _controller);
            }
        }
    }
}