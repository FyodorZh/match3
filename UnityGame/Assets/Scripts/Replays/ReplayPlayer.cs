using Match3;

namespace Replays
{
    public class ReplayPlayer : IGameController
    {
        private ReplayData _data;
        private IGameController _controller;

        private int count = 0;

        public ReplayPlayer(ReplayData data, IGameController controller)
        {
            _data = data;
            _controller = controller;
        }

        public void Start()
        {
            _data.Run(count++, _controller);
        }

        public void Tick(int dTimeMs)
        {
            _data.Run(count++, _controller);
        }

        public void Action(string actionFeatureName, params CellId[] cells)
        {
        }
    }
}