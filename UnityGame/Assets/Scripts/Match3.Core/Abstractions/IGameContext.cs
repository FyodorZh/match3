using System.Collections.Generic;

namespace Match3
{
    public interface IGameContext
    {
        Time CurrentTime { get; }

        int GetRandom();
        void RandomShuffle<T>(IList<T> inOutList);
    }
}