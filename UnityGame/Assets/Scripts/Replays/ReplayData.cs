using System.Collections;
using System.Collections.Generic;
using Match3;

namespace Replays
{
    public abstract class ReplayStep
    {
        public abstract void Invoke(IGameController controller);
    }

    public class StartStep : ReplayStep
    {
        public override void Invoke(IGameController controller)
        {
            controller.Start();
        }
    }

    public class TickStep : ReplayStep
    {
        private readonly int _dTime;

        public TickStep(int dTime)
        {
            _dTime = dTime;
        }

        public override void Invoke(IGameController controller)
        {
            controller.Tick(_dTime);
        }
    }

    public class ActionStep : ReplayStep
    {
        private readonly string _actionFeatureName;
        private readonly CellId[] _cells;

        public ActionStep(string actionFeatureName, params CellId[] cells)
        {
            _actionFeatureName = actionFeatureName;
            _cells = cells;
        }

        public override void Invoke(IGameController controller)
        {
            controller.Action(_actionFeatureName, _cells);
        }
    }

    public class ReplayData
    {
        private readonly List<ReplayStep> _steps = new List<ReplayStep>();

        public int Size => _steps.Count;

        public ReplayData()
        {
        }

        public void Append(ReplayStep step)
        {
            _steps.Add(step);
        }

        public ReplayData(ReplayData source)
        {
            _steps.AddRange(source._steps);
        }

        public void Run(int pos, IGameController controller)
        {
            if (pos < _steps.Count)
                _steps[pos].Invoke(controller);
        }
    }
}