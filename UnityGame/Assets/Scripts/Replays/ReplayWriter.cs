using Match3;

namespace Replays
{
    public class ReplayWriter : IGameController
    {
        private readonly IGameController _controller;

        private readonly ReplayData _replay = new ReplayData();

        public int TickId => _replay.Size;

        public ReplayWriter(IGameController controller)
        {
            _controller = controller;
        }

        public ReplayData GetReplay()
        {
            return new ReplayData(_replay);
        }

        public void Start()
        {
            _replay.Append(new StartStep());
            _controller.Start();
        }

        public void Tick(DeltaTime dTime)
        {
            _replay.Append(new TickStep(dTime));
            _controller.Tick(dTime);
        }

        public void Action(string actionFeatureName, params CellPosition[] cells)
        {
            _replay.Append(new ActionStep(actionFeatureName, cells));
            _controller.Action(actionFeatureName, cells);
        }
    }
}